using Godot;
using System;

public partial class InfoPanel : ColorRect {
    public override void _Process(double delta) {
        Label ElementLabel = GetNode<Label>("BoxContainer/Element");
        Label IsotopeLabel = GetNode<Label>("BoxContainer/Isotope");
        Label ChargeLabel = GetNode<Label>("BoxContainer/Charge");

        Globals.Charge = Globals.ProtonCount - Globals.ElectronCount;
        string ChargeText;
        if (Globals.Charge > 0) {
            ChargeText = $"Charge: +{Globals.Charge}";
        } else {
            ChargeText = $"Charge: {Globals.Charge}";
        }

        ChargeLabel.Text = ChargeText;
    }
}
