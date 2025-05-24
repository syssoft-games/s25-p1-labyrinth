using Godot;

namespace LabyrinthExplorer3D.scripts.game.time;

[GlobalClass]
public partial class TimeController : Node3D
{
    public static TimeController Instance { get; private set; }
    
    public delegate void TimeChangedDelegate(int day, double time, double timePerDay);
    
    private event TimeChangedDelegate _TimeChanged;
    public event TimeChangedDelegate TimeChanged
    {
        add => _TimeChanged += value;
        remove => _TimeChanged -= value;
    }
    
    [Export] public Node3D LightsRoot;
    
    [Export] public double SecondsPerDay = 60; //86.400
    [Export] public Curve RotationCurve;
    
    [Export] public int CurrentDay = 0;
    [Export] public double CurrentTime = 0;

    public void SetTime(int day, double timeInSec)
    {
        CurrentDay = day + (int)(timeInSec / SecondsPerDay);
        CurrentTime = timeInSec % SecondsPerDay;
        
        _TimeChanged?.Invoke(day, timeInSec, SecondsPerDay);
    }
    
    private void _UpdateTime(double delta)
    {
        CurrentTime += delta;
        if (CurrentTime >= SecondsPerDay)
        {
            CurrentDay++;
            CurrentTime -= SecondsPerDay;
        }
        _TimeChanged?.Invoke(CurrentDay, CurrentTime, SecondsPerDay);
    }

    private void _UpdateLightsRotation()
    {
        var globalRotDeg = LightsRoot.GlobalRotationDegrees;
        
        //LERP
        // var rotX = (float)(CurrentTime / SecondsPerDay) * 360f;
        
        //FROM CURVE:
        var rotX = RotationCurve.Sample((float)(CurrentTime / SecondsPerDay));
        globalRotDeg.X = rotX;
        LightsRoot.GlobalRotationDegrees = globalRotDeg;
    }

    public override void _PhysicsProcess(double delta)
    {
        _UpdateTime(delta);
        _UpdateLightsRotation();
    }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
}