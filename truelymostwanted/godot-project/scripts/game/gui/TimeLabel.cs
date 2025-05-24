using Godot;
using LabyrinthExplorer3D.scripts.game.time;

namespace LabyrinthExplorer3D.scripts.game.gui;

[GlobalClass]
public partial class TimeLabel : Label
{
    private void _OnTimeChanged(int day, double time, double timePerDay) //example: Day 1, time: 80sec, timePerDay: 240sec
    {
        var dayText = $"Day {day}";

        var hourUnit = timePerDay / 24;         //e.g. 240sec / 24 => 10sec is 1hour
        var minuteUnit = hourUnit / 60;         //e.g. 10sec / 60 => 0.16sec is 1min
        
        var hours = (int) (time / hourUnit);
        
        var remainingTime = time % hourUnit;
        var minutes = (int) (remainingTime / minuteUnit);
        
        var timeText = $"{hours:00}:{minutes:00}";
        
        Text = $"{dayText}, {timeText}";
    }
    public override void _Ready()
    {
        base._Ready();
        TimeController.Instance.TimeChanged += _OnTimeChanged;
    }
}