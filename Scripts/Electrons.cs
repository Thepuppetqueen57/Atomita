using Godot;
using System;

public partial class Electrons : Node3D {
    [Export] public Node3D CenterNode;
    [Export] public float RotationRadius = 1.0f;
    [Export] public float RotationSpeed = 2.0f; // Radians per second

    public float CurrentAngle = 0.0f; 

    public override void _Ready() {
        CenterNode ??= GetNodeOrNull<Node3D>("/root/3DView/Atom/Nucleus");
        if (CenterNode == null) {
            GD.PrintErr("CenterNode could not be found!");
        }
    }

    public override void _Process(double delta) {
        if (CenterNode == null) return;

        CurrentAngle += RotationSpeed * (float)delta;
        CurrentAngle = Mathf.PosMod(CurrentAngle, Mathf.Tau);

        float x = RotationRadius * MathF.Cos(CurrentAngle);
        float z = RotationRadius * MathF.Sin(CurrentAngle);

        GlobalPosition = CenterNode.GlobalPosition + new Vector3(x, 0, z);
    }
}