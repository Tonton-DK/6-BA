namespace ClassLibrary.Classes;

public class Address
{
    public Address(string road, string number, string zip)
    {
        Road = road;
        Number = number;
        Zip = zip;
    }

    public string Road { get; set; }
    public string Number { get; set; }
    public string Zip { get; set; }
}