using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication1.Models;

public class TrainSchedule : TrainInfo
{
    public int TrainAmount{ get; set; }
    public VectorClass<TrainInfo> Schedule = new();
    public TrainSchedule()
    {
        TrainAmount = 0;
    }

    public TrainInfo ClosestTrain(string destination)
    {
        TrainInfo? min = null;
        int currentIndex = 0;

        var iterator = Schedule.GetIterator(); // Получаем итератор

        foreach (TrainInfo train in Schedule)
        {
            if (train.GetDestination() == destination)
            {
                min = train;
                break;
            }
            currentIndex++;
        }

        if (min != null && currentIndex != Schedule.GetCapacity())
        {
            iterator = Schedule.GetIterator(); // Сбрасываем итератор
            for (int i = 0; i <= currentIndex; i++) // Сдвигаем итератор на текущий индекс
            {
                iterator.MoveNext();
            }

            while (true)
            {
                var currentTrain = iterator.Current;
                if (!(min.GetDepartureTime() < currentTrain.GetDepartureTime()) && currentTrain.GetDestination() == destination)
                {
                    min = currentTrain;
                }
                if (!iterator.MoveNext()) // Переходим к следующему элементу, если есть, иначе прерываем цикл
                    break;
            }
            return min;
        }
        else if (currentIndex == Schedule.GetCapacity())
        {
#pragma warning disable CS8603 // Possible null reference return.
            return min;
#pragma warning restore CS8603 // Possible null reference return.
        }
        else
        {
            throw new ArgumentOutOfRangeException(paramName: nameof(min),
                message:"No such train");
        }
    }


    public List<TrainInfo> FindTrainByDepartureTime(Time t)
    {
        List<TrainInfo> trains = new();
        foreach (var train in Schedule)
        {
            if(train.GetDepartureTime() == t)
            {
                trains.Add(train);
            }
        }
        if (trains.Count > 0)
            return trains;
        throw new ArgumentOutOfRangeException(paramName: nameof(t),
            message:"No such train");
    }
    public void AddTrainInfos(TrainInfo t) => Schedule.Push(t);

}