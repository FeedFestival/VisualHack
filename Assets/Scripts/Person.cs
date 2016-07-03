using SQLite4Unity3d;

public class Person  {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string Name { get; set; }
	public string Surname { get; set; }
	public int Age { get; set; }

	public override string ToString ()
	{
		return string.Format ("[Person: Id={0}, Name={1},  Surname={2}, Age={3}]", Id, Name, Surname, Age);
	}
}

public class User
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Maps { get; set; }
    public bool IsUsingSound { get; set; }
    public int ControllerType { get; set; }

    public override string ToString()
    {
        return string.Format("[User: Id={0}, Name={1}, Maps={2}, IsUsingSound={3}, ControllerType={4}]", Id, Name, Maps, IsUsingSound, ControllerType);
    }
}