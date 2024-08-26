namespace GameClass
{
	public class MapData
	{
		public string ownerid;

		public string did;

		public string date;

		public string title;

		public string url;

		public int sv_index;

		public MapData(string ownerid, string did, string date, string title, string url, int sv_index)
		{
			this.ownerid = ownerid;
			this.did = did;
			this.date = date;
			this.title = title;
			this.url = url;
			this.sv_index = sv_index;
		}

		~MapData()
		{
		}
	}
}
