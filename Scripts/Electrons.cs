using Godot;
using System;

public partial class Electrons : Node3D {
    [Export] public Node3D CenterNode;
    [Export] public float RotationRadius = 1.0f;
    [Export] public float RotationSpeed = 2.0f; // Radians per second

    private float _currentAngle = 0.0f;

    public override void _Ready() {
        // Fallback if not set in the inspector
        CenterNode ??= GetNodeOrNull<Node3D>("/root/3DView/Atom/Nucleus");
        
        if (CenterNode == null) {
            GD.PrintErr("CenterNode could not be found! Make sure the path is correct.");
        }
    }

    public override void _Process(double delta) {
        if (CenterNode == null) return;

        _currentAngle += RotationSpeed * (float)delta;

        _currentAngle = Mathf.PosMod(_currentAngle, Mathf.Tau);

        float x = RotationRadius * MathF.Cos(_currentAngle);
        float z = RotationRadius * MathF.Sin(_currentAngle);

        GlobalPosition = CenterNode.GlobalPosition + new Vector3(x, 0, z);
    }
}