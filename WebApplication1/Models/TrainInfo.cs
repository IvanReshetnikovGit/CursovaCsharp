using System.Security.Cryptography.X509Certificates;

namespace WebApplication1.Models;

public class TrainInfo :  Time
{
    private string? Destination{ get; set; }
    private Time DepartureTime{ get; set; }
    private int Peron{ get; set; }
    public TrainInfo(string destination, Time departureTime, int peron)
    {
        SetDestination(destination);
        DepartureTime = departureTime;
        SetPeron(peron);
    }
    public TrainInfo()
    {
        Destination = "";
        DepartureTime = new Time();
    }

    public void SetDestination(string destination)
    {
        if(destination.All(char.IsLetterOrDigit))
        {
            Destination = destination.ToUpper();
        }
        else
        {
            throw new ArgumentException(paramName: nameof(destination), message:"Desatination contains invalid characters.");
        }
    }
    public void  SetDepartureTime(Time departureTime)
    {
        DepartureTime = departureTime;
    }
    public void SetPeron(int peron)
    {
        if (peron > 0)
            Peron = peron;
        else
            throw new ArgumentOutOfRangeException(paramName: nameof(peron),message:"Peron number should bigger than 0");
    }
    public string GetDestination()
    {
        return Destination;
    }
    public Time GetDepartureTime()
    {
        return DepartureTime;
    }
    public int GetPeron()
    {
        return Peron;
    }

    public override string GetTime()  
    {
        return $"{DepartureTime}" + "From TrainInfo\n";
    }
}