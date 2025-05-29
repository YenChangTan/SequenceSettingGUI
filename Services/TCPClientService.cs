using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using SequenceSettingGUI.Interfaces;

namespace SequenceSettingGUI.Services;

public class TCPClientService : ITCPClientService
{
    private readonly object _lock = new();
    public string IP { get; set; }

    public int Port { get; set; }

    private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private readonly string[] CommandStr = new string[]{
        "Rd_Input",//0
        "Wr_Outpu",//1
        "Rly00Off",//2
        "Rly00Onn"//3
    };


    public bool SetIPAddress(string IP, int Port)
    {
        this.IP = IP;
        this.Port = Port;
        return true;
    }


    public bool Connect()
    {
        lock (_lock)
        {
            try
            {
                socket.Connect(IPAddress.Parse(IP), Port);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public bool SendAndReceiveEcho(byte[] Command)
    {
        byte[] ReceivedEcho = new byte[Command.Length];
        int SentByteCount = socket.Send(Command);
        if (Command.Length != SentByteCount)
        {
            return false;
        }
        int ReceivedByteCount = socket.Receive(ReceivedEcho);
        if (ReceivedByteCount != SentByteCount)
        {
            return false;
        }
        if (!ReceivedEcho.SequenceEqual(Command))
        {
            return false;
        }
        return true;
    }

    public bool WriteOutput(bool On, int OutputIndex)
    {
        lock (_lock)
        {
            if (OutputIndex >= 0 && OutputIndex < 16)
            {
                OutputIndex += 1;
                int CommandIndex = !On ? 2 : 3;
                List<byte> CommandList = [.. Encoding.ASCII.GetBytes(CommandStr[CommandIndex])];
                CommandList[3] = (byte)('0' + (OutputIndex / 10));
                CommandList[4] = (byte)('0' + (OutputIndex % 10));
                CommandList.AddRange(CRCGenerator(CommandList));
                byte[] Command = CommandList.ToArray();
                bool result = SendAndReceiveEcho(Command);
                return false;
            }
            else
            {
                return false;
            }
        }
    }


    public bool WriteOutput(bool[] OutputArray)
    {
        lock (_lock)
        {
            if (OutputArray.Length == 16)
            {
                Debug.WriteLine("Write Output Successfully");
                List<byte> CommandList = [.. Encoding.ASCII.GetBytes(CommandStr[1])];
                CommandList.AddRange(OutputArrayToTwoByte(OutputArray));
                CommandList.AddRange(CRCGenerator(CommandList));
                byte[] Command = CommandList.ToArray();
                bool result = SendAndReceiveEcho(Command);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool[] ReadInput()
    {
        lock (_lock)
        {
            List<byte> CommandList = [.. Encoding.ASCII.GetBytes(CommandStr[0])];
            CommandList.AddRange(CRCGenerator(CommandList));
            byte[] Command = CommandList.ToArray();
            bool result = SendAndReceiveEcho(Command);
            byte[] BytesReceive = new byte[14];
            int ReceivedByteCount = socket.Receive(BytesReceive);

            if (ReceivedByteCount != 14)
            {
                if (ReceivedByteCount < 14)
                {
                    Console.WriteLine("less than 14");
                    Console.WriteLine(ByteArrayToHex(BytesReceive));
                    //throw new Exception();
                }
                else
                {
                    Console.WriteLine("more than 14");
                }
            }
            if (Encoding.ASCII.GetString(BytesReceive, 0, 8) != CommandStr[0])
            {
                throw new Exception();
            }
            // if (!CRCGenerator(BytesReceive.ToList()).SequenceEqual(new List<Byte> { 0x00, 0x00 }))
            // {
            //     Console.WriteLine(ByteArrayToHex(CRCGenerator(BytesReceive.ToList()).ToArray()));
            //     Console.WriteLine(CRCGenerator(BytesReceive.ToList()).SequenceEqual(new List<Byte> { 0x00, 0x00 }));
            //     Console.WriteLine(ByteArrayToHex(BytesReceive));
            //     throw new Exception();
            // }
            bool[] InputData = new bool[16];
            for (int i = 0; i < 16; i++)
            {
                if (((BytesReceive[8 + i / 8] >> (i % 8)) & 1) == 0x01)
                {
                    InputData[i] = true;
                }
            }
            Console.WriteLine("right at 14");
            Console.WriteLine(ByteArrayToHex(BytesReceive));
            return InputData;
        }
    }

    public bool Disconnect()
    {
        lock (_lock)
        {
            socket.Disconnect(true);
            return false;
        }
    }

    public bool ValidateEcho(byte[] Sent, byte[] Receive)//To check the echo message is as expected
    {
        return false;
    }


    public List<byte> OutputArrayToTwoByte(bool[] OutputArray)
    {
        List<byte> TwoByte = new List<byte> { 0x00, 0x00 };
        for (int i = 0; i < 16; i++)
        {
            if (OutputArray[i])
            {
                TwoByte[i / 8] |= (byte)(1 << (i % 8));
            }
        }

        return TwoByte;
    }



    public List<byte> CRCGenerator(List<byte> CommandWithoutCRC)
    {
        ushort crc = 0xFFFF;
        foreach (byte element in CommandWithoutCRC)
        {
            crc ^= element;
            for (int i = 0; i < 8; i++)
            {
                if ((crc & 0x0001) == 1)
                {
                    crc >>= 1;
                    crc ^= 0xA001;
                }
                else
                {
                    crc >>= 1;
                }
            }
        }
        List<byte> CRC = new List<byte>() { (byte)(crc & 0xFF), (byte)((crc >> 8) & 0xFF) };
        return CRC;
    }

    public string ByteArrayToHex(byte[] bytes)
    {
        string separator = " ";
        StringBuilder hex = new StringBuilder(bytes.Length * (2 + separator.Length));
        for (int i = 0; i < bytes.Length; i++)
        {
            if (i > 0)
            {
                hex.Append(separator);
            }
            hex.AppendFormat("{0:x2}", bytes[i]);
        }
        return hex.ToString();
    }
    
    public string ByteArrayToHex(byte[] bytes, int toChange)
    {
        int Length = Math.Min(toChange, bytes.Length);
        string separator = " ";
        StringBuilder hex = new StringBuilder(Length * (2 + separator.Length));
        for (int i = 0; i < Length; i++)
        {
            if (i > 0)
            {
                hex.Append(separator);
            }
            hex.AppendFormat("{0:x2}", bytes[i]);
        }
        return hex.ToString();
    }
}
