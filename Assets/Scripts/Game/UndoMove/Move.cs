public class Move
{
	public Screw fromScrew;

	public Screw toScrew;

	public int number;

	public Move(Screw from = null, Screw to = null, int num = 0)
	{
		this.fromScrew = from;
		this.toScrew = to;
		this.number = num;
	}
}