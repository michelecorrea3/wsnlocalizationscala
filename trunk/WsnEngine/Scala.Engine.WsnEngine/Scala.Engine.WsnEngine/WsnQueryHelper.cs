using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using System.Net.Sockets;
using System.Data;
using System.ComponentModel;
using System.ServiceModel;
using System.Diagnostics;
using System.Xml.Linq;

using SocketConnection;
using DatabaseConnection;
using Elab.Rtls.Engines.WsnEngine.Positioning;

using Elab.Toolkit.Core.Xml;
using Scala.Core;

namespace Elab.Rtls.Engines.WsnEngine
{
    public static class WsnQueryHelper
    {
        /// <summary>
        /// Processes the Query 
        /// Retreives the data from database
        /// For use in IQueryable
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>QueryResult</returns>
        public static QueryResponse Query(Query query, MySQLClass MyDB)
        {
            //decode the Query and perform the SQL queries accordingly
            StringBuilder QueryCmd = new StringBuilder("", 3000);
            DataSet ReturnSet;

            //Query consists of 4 elements: 
            //QueryName: this is always the same in the API and can be ignored for now

            //Ignore the fields in the SQL request, just request them all for now...
            //Fields
            QueryCmd.Append(
                "select idnode, sensor, name, button1, temperature, X, Y, Z  from (select idnode, sensor, name from node) as nd inner join (select * from (select button1, temperature, max(time) as senstime, node as sensnode from sensormeasurements group by node) as sens left outer join (select X, Y, Z, max(time) as loctime, node as locnode from localization group by node) as loc on sens.sensnode = loc.locnode union select * from (select button1, temperature, max(time) as senstime, node as sensnode from sensormeasurements group by node) as sens right outer join (select X, Y, Z, max(time) as loctime, node as locnode from localization group by node) as loc on sens.sensnode = loc.locnode) as sq on nd.idnode = sq.sensnode");

            if (query.Filters != null)
            {
            int i = 1, j = 1;

            foreach (string filter in query.Filters.Keys)
            {
                List<string> filterValues = query.Filters[filter];

                switch (filter)
                {
                    case "Serial":
                        QueryCmd.Append(" where ");

                        foreach (string filterValue in filterValues)
                        {
                            QueryCmd.Append("sensor = " + filterValue);
                            if (j != filterValues.Count)
                            {
                                QueryCmd.Append(" OR ");
                                j++;
                            }
                        }
                        j = 1;
                        break;
                    case "TagID":
                        QueryCmd.Append(" where ");

                        foreach (string filterValue in filterValues)
                        {
                            QueryCmd.Append("idnode = " + filterValue);
                            if (j != filterValues.Count)
                            {
                                QueryCmd.Append(" OR ");
                                j++;
                            }
                        }
                        j = 1;
                        break;
                    case "Name":
                        QueryCmd.Append(" where ");

                        foreach (string filterValue in filterValues)
                        {
                            QueryCmd.Append("name = " + filterValue);
                            if (j != filterValues.Count)
                            {
                                QueryCmd.Append(" OR ");
                                j++;
                            }
                        }
                        j = 1;
                        break;
                    case "":
                        break;
                    default:
                        throw new FaultException("The filter" + filter.ToString() + "is not implemented yet");
                }
                if (i < query.Filters.Keys.Count)
                    QueryCmd.Append(" AND ");
                i++;
            }
        }

            //Sortby
            if (query.SortBy != null && query.SortBy != "" && query.SortBy != "asc" && query.SortBy != "desc")
                QueryCmd.Append(" order by " + query.SortBy + ";");
            else
                QueryCmd.Append(";");

            //assemble the SQL results into TagBlinks
            //parse them into the correct type
            TagBlinks tagBlinks = new TagBlinks();
            string TempString = QueryCmd.ToString();

            try
            {
                //Execute the Query
                ReturnSet = MyDB.Query(TempString);

                //determine the UTC offset
                int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
                string timezone = hours >= 0 ? " +" + hours : " " + hours;

                //one for each TagBlink
                foreach (DataRow Row in ReturnSet.Tables[0].Rows)
                {
                    //declare a single tagblink
                    TagBlink tagBlink = new TagBlink();

                    foreach (string field in query.QueryFields)
                    {
                        switch (field)
                        {
                            //convert to the correct formats
                            //add the fields to the Query (select)
                            case "TagID":
                                tagBlink["TagID"] = Row["idnode"].ToString();
                                break;
                            case "Name":
                                tagBlink["Name"] = Row["name"].ToString();
                                break;
                            case "Serial":
                                tagBlink["Serial"] = Row["sensor"].ToString();
                                break;
                            case "Location":
                                tagBlink["Location/X"] = Row["X"].ToString();
                                tagBlink["Location/Y"] = Row["Y"].ToString();
                                break;
                            case "Location/X":
                                tagBlink["Location/X"] = Row["X"].ToString();
                                break;
                            case "Location/Y":
                                tagBlink["Location/Y"] = Row["Y"].ToString();
                                break;
                            case "Buttons":
                                //skip if this boolean is 0
                                tagBlink["Buttons"] = Row["button1"].ToString();
                                break;
                            case "Temperature":
                                tagBlink["Temperature"] = Row["temperature"].ToString();
                                break;
                            case "Light":
                                tagBlink["Light"] = Row["light"].ToString();
                                break;
                            case "Humidity":
                                tagBlink["Humidity"] = Row["humidity"].ToString();
                                break;
                            case "RTLSBlinkTime":
                                tagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                                break;
                            case "LocateTime":
                                tagBlink["LocateTime"] = Row["loctime"].ToString().Replace('-', '/') + timezone;
                                break;
                            case "TagModel":
                                tagBlink["TagModel"] = "Telosb";
                                break;
                            case "Technology":
                                tagBlink["Technology"] = "TelosbWsn";
                                break;
                            case "All":
                                tagBlink["TagID"] = Row["idnode"].ToString();
                                tagBlink["Name"] = Row["name"].ToString();
                                tagBlink["Serial"] = Row["sensor"].ToString();
                                tagBlink["Location/X"] = Row["X"].ToString();
                                tagBlink["Location/Y"] = Row["Y"].ToString();
                                tagBlink["Buttons"] = Row["button1"].ToString();
                                tagBlink["Temperature"] = Row["temperature"].ToString();
                                tagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                                tagBlink["Light"] = Row["light"].ToString();
                                tagBlink["Humidity"] = Row["humidity"].ToString();
                                tagBlink["LocateTime"] = Row["loctime"].ToString().Replace('-', '/') + timezone;
                                break;
                            default:
                                throw new FaultException("The field " + field.ToString() + " is not available");
                        }
                    }
                    tagBlinks.Add(tagBlink);
                }
            }
            //Jerry won't like this...
            catch (FaultException fex)
            {
                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.StackTrace);
                throw fex;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw new FaultException("The database of the WSN could not be accessed properly or the SQL query was unable to execute\n Check the connection parameters");
            }

            if (tagBlinks.Count == 0 || tagBlinks == null)
                throw new FaultException("No tagBlinks availaible for " + query.ToString());

            QueryResult queryResult = new QueryResult(tagBlinks);
            return new QueryResponse(queryResult);
        }

        /// <summary>
        /// Processes the Query 
        /// Retreives the data from the events
        /// For use in IEventSource
        /// </summary>
        /// <param name="query">Query</param>
        /// <returns>Whether the Query succeeded or not</returns>
        public static bool TryQuery(EventSubscription eventSubscription, TagBlink tagBlink, out TagBlink ReturnTagBlink)
        {
            ReturnTagBlink = null;
            int hours = TimeZoneInfo.Local.BaseUtcOffset.Hours;
            string timezone = hours >= 0 ? " +" + hours : " " + hours;

            //filters
            foreach (string filterKey in eventSubscription.Filters.Keys)
            {   
                List<string> filterValues = eventSubscription.Filters[filterKey];
                bool availaible = false;
                try
                {
                    switch (filterKey)
                    {

                        case "TagId" :
                            foreach (string filterValue in filterValues)
                            {
                                if (tagBlink["TagID"] == filterValue)
                                    availaible = true;
                            }
                            if (!availaible)
                                return false;
                            break;
                        case "":
                            break;
                        default :
                            throw new FaultException("Filtering on: " + filterKey + " is not supported yet in WsnEngine");
                    }
                }
                //TODO: catch more specific exception
                catch (Exception)
                {
                    throw new FaultException("Filtering on: " + filterKey + " is not supported for an " + eventSubscription.EventType + " event.");
                }
            }

            //fields!!
            foreach (string field in eventSubscription.Fields)
            {
                try
                {
                    switch (field)
                    {
                        //convert to the correct formats
                        //add the fields to the Query (select)
                        case "TagID":
                            ReturnTagBlink["TagID"] = tagBlink["idnode"];
                            break;
                        case "Name":
                            ReturnTagBlink["Name"] = tagBlink["name"];
                            break;
                        case "Serial":
                            ReturnTagBlink["Serial"] = tagBlink["sensor"];
                            break;
                        case "Location":
                            ReturnTagBlink["Location/X"] = tagBlink["X"];
                            ReturnTagBlink["Location/Y"] = tagBlink["Y"];
                            break;
                        case "Location/X":
                            ReturnTagBlink["Location/X"] = tagBlink["X"];
                            break;
                        case "Location/Y":
                            ReturnTagBlink["Location/Y"] = tagBlink["Y"];
                            break;
                        case "Buttons":
                            //skip if this boolean is 0
                            ReturnTagBlink["Buttons"] = tagBlink["Button"];
                            break;
                        case "Temperature":
                            ReturnTagBlink["Temperature"] = tagBlink["temperature"];
                            break;
                        case "Light":
                            ReturnTagBlink["Light"] = tagBlink["Light"];
                            break;
                        case "Humidity":
                            ReturnTagBlink["Humidity"] = tagBlink["Humidity"];
                            break;
                        case "RTLSBlinkTime":
                            ReturnTagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                            break;
                        case "All":
                            //switch on the type of event
                            switch (eventSubscription.EventType)
                            {
                                case "TemperatureChanged":
                                    ReturnTagBlink["TagID"] = tagBlink["idnode"];
                                    ReturnTagBlink["Temperature"] = tagBlink["Temperature"];
                                    ReturnTagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                                    break;
                                case "LightChanged":
                                    ReturnTagBlink["TagID"] = tagBlink["idnode"];
                                    ReturnTagBlink["Light"] = tagBlink["Light"];
                                    ReturnTagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                                    break;
                                case "HumidityChanged":
                                    ReturnTagBlink["TagID"] = tagBlink["idnode"];
                                    ReturnTagBlink["Humidity"] = tagBlink["Humidity"];
                                    ReturnTagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                                    break;
                                case "ButtonPressed":
                                    ReturnTagBlink["TagID"] = tagBlink["idnode"];
                                    ReturnTagBlink["Button"] = tagBlink["Button"];
                                    ReturnTagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                                    break;
                                case "LocationUpdated":
                                    ReturnTagBlink["TagID"] = tagBlink["idnode"];
                                    ReturnTagBlink["Location/X"] = tagBlink["X"];
                                    ReturnTagBlink["Location/Y"] = tagBlink["Y"];
                                    ReturnTagBlink["RTLSBlinkTime"] = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + timezone;
                                    break;
                            }
                            break;
                        default:
                            throw new FaultException("The field" + field.ToString() + " is not provided in the WSN engine");
                    }
                }
                //TODO: catch more specific exception
                catch (Exception)
                {
                    throw new FaultException("The field" + field.ToString() + " is not provided in the " + eventSubscription.EventType + " event.");
                }
            }
            return true;
        }
    }
}
