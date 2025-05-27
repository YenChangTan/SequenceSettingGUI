namespace SequenceSettingGUI.Interfaces;

public interface ITCPClientService
{
    public bool SetIPAddress(string IP, int Port);
    public bool Connect();
    public bool WriteOutput(bool On, int Index);
    public bool WriteOutput(bool[] OutputArray);
    public bool[] ReadInput();
    public bool Disconnect();
    
}
