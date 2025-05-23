using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace SequenceSettingGUI.Services;

public class TCPClientService
{
    public string IP{ get; set; }

    public int Port{ get; set; }

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

    public bool Send(byte[] Command)
    {
        socket.Send(Command);
        return true;
    }

    public bool Receive()
    {
        
        return false;
    }

    

    public async Task<bool> SendAndReceiveEcho(byte[] Command)
    {
        byte[] ReceivedEcho = new byte[Command.Length];
        int SentByteCount = await socket.SendAsync(Command);
        if (Command.Length != SentByteCount)
        {
            return false;
        }
        int ReceivedByteCount = await socket.ReceiveAsync(ReceivedEcho);
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

    public async Task<bool> WriteOutput(int OutputIndex, bool On)
    {
        if (OutputIndex >= 0 && OutputIndex < 16)
        {
            
            int CommandIndex = On ? 2 : 3;
            List<byte> CommandList = [.. Encoding.ASCII.GetBytes(CommandStr[CommandIndex])];
            CommandList[3] = (byte)('0' + (OutputIndex / 10));
            CommandList[4] = (byte)('0' + (OutputIndex % 10));
            CommandList.AddRange(CRCGenerator(CommandList));
            byte[] Command = CommandList.ToArray();
            bool result = await SendAndReceiveEcho(Command);
            return false;
        }
        else
        {
            return false;
        }
    }


    public async Task<bool> WriteOutput(bool[] OutputArray)
    {
        if (OutputArray.Length != 16)
        {

            List<byte> CommandList = [.. Encoding.ASCII.GetBytes(CommandStr[1])];
            CommandList.AddRange(OutputArrayToTwoByte(OutputArray));
            CommandList.AddRange(CRCGenerator(CommandList));
            byte[] Command = CommandList.ToArray();
            bool result = await SendAndReceiveEcho(Command);
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<byte[]> ReadInput()
    {
        List<byte> CommandList = [.. Encoding.ASCII.GetBytes(CommandStr[0])];
        CommandList.AddRange(CRCGenerator(CommandList));
        byte[] Command = CommandList.ToArray();
        bool result = await SendAndReceiveEcho(Command);
        byte
        socket.Receive()
        return new byte[] { };
    }

    public bool Disconnect()
    {
        return false;
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
                TwoByte[i / 8] |= (byte) (1 << (i % 8));
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
}
