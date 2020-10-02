using System.Text;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class StringParser : IStringParser
    {
        public string BuildUpdatedVesselInfo(VesselUpdateModel result)
        {
            return BuildResult(result);
        }

        private string BuildResult(VesselUpdateModel result)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(result.IMO);
            sb.Append(" | ");
            if (result.AISLatestActivity.HasValue)
            {
                sb.Append(result.AISLatestActivity.Value.ToShortDateString());
            }
            else
            {
                sb.Append("(no activity time)");
            }
            sb.Append(" | ");
            if (!string.IsNullOrEmpty(result.AISStatus))
            {
                sb.Append(result.AISStatus);
            }
            else
            {
                sb.Append("(no status)");
            }
            sb.Append(" | ");
            if (!string.IsNullOrEmpty(result.Destination))
            {
                sb.Append(result.Destination);
            }
            else
            {
                sb.Append("(no destination)");
            }

            return sb.ToString();
        }
    }
}
