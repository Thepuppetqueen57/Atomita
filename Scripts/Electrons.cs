using Godot;
using System;

public partial class Electrons : Node3D {
    [Export] public Node3D CenterNode;
    [Export] public float RotationRadius = 1.0f;
    [Export] public float Speed = 2.0f;
    [Export] public float StartingAngle = 0.0f;

    private float _angle = 0.0f;

    public override void _Ready() {
        CenterNode ??= GetNode<Node3D>("/root/3DView/Atom/Nucleus");
        _angle = Mathf.DegToRad(StartingAngle);
    }

    public override void _Process(double delta) {
        if (CenterNode == null) return;

        _angle += (float)delta * Speed;

        float x = RotationRadius * MathF.Cos(_angle);
        float z = RotationRadius * MathF.Sin(_angle);

        GlobalPosition = CenterNode.GlobalPosition + new Vector3(x, 0, z);
    }
}
