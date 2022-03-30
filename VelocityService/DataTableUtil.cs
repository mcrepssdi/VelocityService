using System.Data;
using VelocityService.Models;

namespace VelocityService;

public static class DataTableUtil
{
    public static DataTable GetDataTable()
    {
        DataTable dt = new ();
        dt.Columns.Add("Guid", typeof(string));
        dt.Columns.Add("InitialVelocity", typeof(double));
        dt.Columns.Add("Acceleration", typeof(double)); 
        dt.Columns.Add("Time", typeof(double)); 
        dt.Columns.Add("Rpms", typeof(int)); 
        dt.Columns.Add("RunDurationSeconds", typeof(int));
        dt.Columns.Add("EntryDateTime");
        return dt;
    }

    public static DataRow BuildDataRow(this DataTable dt, Velocity v, DateTime st)
    {
        DataRow row = dt.NewRow();
        row["Guid"] = v.Guid;
        row["InitialVelocity"] = v.InitialVelocity;
        row["Acceleration"] = v.Acceleration; 
        row["Time"] = v.Time; 
        row["Rpms"] = v.Rpms; 
        
        TimeSpan ts = DateTime.Now - st;
        row["RunDurationSeconds"] = ts.Milliseconds;
        row["EntryDateTime"] = DateTime.Now;
        return row;
    }
}