using System.Reflection.Metadata.Ecma335;

namespace OrderDB;

public partial class Sequence
{
    public int Id { get; set; }
    
    public required string OwnerId {get;set;}

    public int NoOrder{get;set;}
}