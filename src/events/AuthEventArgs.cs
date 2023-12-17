using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JachtZ.src.events
{
	public class AuthEventArgs : EventArgs
	{
		public bool isAuth { get; set; }
		public string username { get; set; }
	
		public AuthEventArgs(string username)
		{
			this.username = username;
		}
	}
}
