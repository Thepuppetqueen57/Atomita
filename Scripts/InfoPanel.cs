using Godot;
using System;
using System.Collections.Generic;

public partial class InfoPanel : ColorRect {
    List<string> Elements = [
        "Unknown",
        "Hydrogen",
        "Helium",
        "Lithium",
        "Beryllium",
        "Boron",
        "Carbon",
        "Nitrogen",
        "Oxygen",
        "Fluorine",
        "Neon",
        "Sodium",
        "Magnesium",
        "Aluminum",
        "Silicon",
        "Phosphorus",
        "Sulfur",
        "Chlorine",
        "Argon",
        "Potassium",
        "Calcium",
        "Scandium",
        "Titanium",
        "Vanadium",
        "Chromium",
        "Manganese",
        "Iron",
        "Cobalt",
        "Nickel",
        "Copper",
        "Zinc",
        "Gallium",
        "Germanium",
        "Arsenic",
        "Selenium",
        "Bromine",
        "Krypton",
        "Rubidium",
        "Strontium",
        "Yttrium",
        "Zirconium",
        "Niobium",
        "Molybdenum",
        "Technetium",
        "Ruthenium",
        "Rhodium",
        "Palladium",
        "Silver",
        "Cadmium",
        "Indium",
        "Tin",
        "Antimony",
        "Tellurium",
        "Iodine",
        "Xenon",
        "Cesium",
        "Barium",
        "Lanthanum",
        "Cerium",
        "Praseodymium",
        "Neodymium",
        "Promethium",
        "Samarium",
        "Europium",
        "Gadolinium",
        "Terbium",
        "Dysprosium",
        "Holmium",
        "Erbium",
        "Thulium",
        "Ytterbium",
        "Lutetium",
        "Hafnium",
        "Tantalum",
        "Tungsten",
        "Rhenium",
        "Osmium",
        "Iridium",
        "Platinum",
        "Gold",
        "Mercury",
        "Thallium",
        "Lead",
        "Bismuth",
        "Polonium",
        "Astatine",
        "Radon",
        "Francium",
        "Radium",
        "Actinium",
        "Thorium",
        "Protactinium",
        "Uranium",
        "Neptunium",
        "Plutonium",
        "Americium",
        "Curium",
        "Berkelium",
        "Californium",
        "Einsteinium",
        "Fermium",
        "Mendelevium",
        "Nobelium",
        "Lawrencium",
        "Rutherfordium",
        "Dubnium",
        "Seaborgium",
        "Bohrium",
        "Hassium",
        "Meitnerium",
        "Darmstadtium",
        "Roentgenium",
        "Copernicium",
        "Nihonium",
        "Flerovium",
        "Moscovium",
        "Livermorium",
        "Tennessine",
        "Oganesson"
    ]; // That took a while to scroll through I'm glad its over

    public override void _Process(double delta) {
        Label ElementLabel = GetNode<Label>("BoxContainer/Element");
        Label IsotopeLabel = GetNode<Label>("BoxContainer/Isotope");
        Label ChargeLabel = GetNode<Label>("BoxContainer/Charge");

        if (Globals.ProtonCount < Elements.Count) {
            Globals.Element = Elements[Globals.ProtonCount];
        } else {
            Globals.Element = "Unknown";
        }

        ElementLabel.Text = $"Element: {Globals.Element}";

        if (Globals.NeutronCount == 0 && Globals.ProtonCount == 1) {
            Globals.Isotope = $"Protium";
        } else if (Globals.NeutronCount == 1 && Globals.ProtonCount == 1) {
            Globals.Isotope = $"Deuterium";
        } else if (Globals.NeutronCount == 2 && Globals.ProtonCount == 1) {
            Globals.Isotope = $"Tritium";
        } else {
            Globals.Isotope = $"{Globals.Element}-{Globals.ProtonCount + Globals.NeutronCount}";
        }

        IsotopeLabel.Text = $"Isotope: {Globals.Isotope}";

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
