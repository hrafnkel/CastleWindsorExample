namespace ClassLibrary1
{
	public class Main
	{
		private IDependency1 object1;
		private IDependency2 object2;

		public Main(IDependency1 dependency1, IDependency2 dependency2)
		{
			object1 = dependency1;
			object2 = dependency2;
		}

		public void DoSomething()
		{
			object1.SomeObject = "Hello World";
			object2.SomeOtherObject = "Hello Mars";
		}
	}
}
