namespace HateoasNet.TestingObjects
{
	public class Testee
	{
		public string StringValue { get; set; } = "test";
		public bool BoolValue { get; set; } = true;
		public int IntegerValue { get; set; } = 100;
		public long LongIntegerValue { get; set; } = 549587541536;
		public decimal DecimalValue { get; set; } = 3289.57m;
		public float FloatValue { get; set; } = 100f;


		public override string ToString()
		{
			return StringValue;
		}
	}
}
