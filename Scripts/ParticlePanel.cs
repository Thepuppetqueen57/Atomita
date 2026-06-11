using Godot;
using System;

public partial class ParticlePanel : ColorRect {
    public override void _Process(double delta) {
        GetNode<Label>("/root/3DView/CanvasLayer/ParticlePanel/Electron/Label").Text = $"Electrons: {Globals.ElectronCount}";
        GetNode<Label>("/root/3DView/CanvasLayer/ParticlePanel/Proton/Label").Text = $"Protons: {Globals.ProtonCount}";
        GetNode<Label>("/root/3DView/CanvasLayer/ParticlePanel/Neutron/Label").Text = $"Neutrons: {Globals.NeutronCount}";
    }
}
