using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Reflection.Metadata;

namespace WebApplication1.Models;
public class Time
{
    private int Hour{get; set;}
    private int Minute{get; set;}
    private int Second{get; set;}

    public void SetHour(int hour)
    {
        if (hour < 0 || hour > 23)
            throw new ArgumentOutOfRangeException(paramName:nameof(hour),message:"Hour must be between 0 and 23");
        
        else
            Hour = hour;
    }
    public void SetMinute(int minute)
    {
        if(minute < 0 || minute > 59)
            throw new ArgumentOutOfRangeException(paramName: nameof(minute),
                message:"Minute must be between 0 and 59");
        else
            Minute = minute;
    }
    public void SetSecond(int second)
    {
        if(second < 0 || second > 59)
            throw new ArgumentOutOfRangeException(paramName: nameof(second),
                message:"Second must be between 0 and 59");
        else
            Second = second;
    }
    public int GetHour()
    {
        return Hour;
    }
    public int GetMinute()
    {
        return Minute;
    }
    public int GetSecond()
    {
        return Second;
    }
    public Time(int hour, int minute,int second)
    {
        SetHour(hour);
        SetMinute(minute);
        SetSecond(second);
    }

   public Time(Time time,bool move)
    {
        Hour = time.Hour;
        Minute = time.Minute;
        Second = time.Second;

        time.Hour = 0;
        time.Minute = 0;
        time.Second = 0;
    }
    public Time(Time time)
    {
        Hour = time.Hour;
        Minute = time.Minute;
        Second = time.Second;
    }
    public Time(){}
    
    public static Time operator +(Time left, Time right)
    {
        int newHour = left.Hour + right.Hour;
        int newMinute = left.Minute + right.Minute;
        int newSecond = left.Second + right.Second;
        if (newSecond >= 60)
        {
            newSecond -= 60;
            newMinute++;
        }
        if (newMinute >= 60)
        {
            newMinute -= 60;
            newHour++;
        }
        return new Time(newHour, newMinute, newSecond);
    }

    public static Time operator -(Time left, Time right)
    {
        int newHour = left.Hour - right.Hour;
        int newMinute = left.Minute - right.Minute;
        int newSecond = left.Second - right.Second;
        if (newSecond < 0)
        {
            newSecond += 60;
            newMinute--;
        }
        if (newMinute < 0)
        {
            newMinute += 60;
            newHour--;
        }
        return new Time(newHour, newMinute, newSecond);
    }
    public void CopyFrom(Time time)
    {
        Hour = time.Hour;
        Minute = time.Minute;
        Second = time.Second;
    }
    public virtual string GetTime()
    {
        string hour = Hour.ToString(),minute = Minute.ToString(),second = Second.ToString();
        if (Hour <= 9)
            hour = "0" + Hour;  
        if(Minute <= 9)
            minute = "0" + Minute;
        if(Second <= 9)
            second = "0" + Second;
    
        return hour + ":" + minute + ":" + second;
    }
    public static bool operator<(Time left, Time right)
    {
            if (left.Hour < right.Hour)
            return true;
        else if (left.Hour > right.Hour)
            return false;
        else
        {
            if (left.Minute < right.Minute)
                return true;
            else if (left.Minute > right.Minute)
                return false;
            else 
                return left.Second < right.Second;
        }
    }
    public static bool operator>(Time left, Time right)
    {
        return !(left < right);
    }

    public static bool operator ==(Time? left, Time? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;

        return left.Hour == right.Hour && left.Minute == right.Minute && left.Second == right.Second;
    }

    public static bool operator !=(Time? left, Time? right)
    {
        return !(left == right);
    }
    public override bool Equals([System.Diagnostics.CodeAnalysis.AllowNull] object obj)
    {
        if (obj is not Time other) return false;

        return Hour == other.Hour && Minute == other.Minute && Second == other.Second;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hour, Minute, Second);
    }
    public bool IsValid(Time other)
    {
        int thisTotalMinutes = Hour * 60 + Minute; 
        int otherTotalMinutes = other.Hour * 60 + other.Minute; 
        
        int timeDifference = Math.Abs(thisTotalMinutes - otherTotalMinutes); 
        
        return timeDifference > 30;
    }
}
