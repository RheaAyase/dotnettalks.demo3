using System;
using System.Linq;
using System.Threading.Tasks;
using Tmds.DBus;
using Systemd1.DBus;

namespace dotnettalk
{
	public class Systemctl
	{
		public static async Task<string> GetServiceStatus(string serviceName)
		{
			string statusString = "";
			using( var connection = new Connection(Address.System) )
			{
				await connection.ConnectAsync();
				IManager systemd = connection.CreateProxy<IManager>("org.freedesktop.systemd1",
					"/org/freedesktop/systemd1");

				Unit[] units = await systemd.ListUnitsAsync();
				Unit unit = units.FirstOrDefault(u => u.Name == serviceName);

				statusString = string.Format("Service Name: `{0}`\n" +
				                             "Status: `{1}` - `{2}`", unit.Name, unit.ActiveState, unit.SubState);
			}

			return statusString;
		}

		public static async Task<bool> RestartService(string serviceName)
		{
			using( Connection connection = new Connection(Address.System) )
			{
				await connection.ConnectAsync();
				IManager systemd = connection.CreateProxy<IManager>("org.freedesktop.systemd1",
					"/org/freedesktop/systemd1");

				await systemd.RestartUnitAsync(serviceName, "fail");
			}
			return true; //TODO: actually read returned job...
		}
	}
}
