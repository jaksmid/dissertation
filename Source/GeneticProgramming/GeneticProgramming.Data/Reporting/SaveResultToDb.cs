using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using GeneticProgramming.Data.Dao;
using log4net;

namespace GeneticProgramming.Data.Reporting
{
    public class SaveResultToDb
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SaveResultToDb));
        private static readonly DbEntitiesProviderFactory DbEntitiesProviderFactory=new DbEntitiesProviderFactory();
        public static bool SaveResult(string name, string result, string type, string settings)
        {
            try
            {
                using (var dbProvider = DbEntitiesProviderFactory.CreateDbEntitiesProvider(false))
                {
                    dbProvider.SaveExperimentResults(name, result, type, settings);
                }
                Logger.Info("Saved results to the database");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("Failed to insert results into the database. Falling back to proxy service",e);
                try
                {
                    var values = new Dictionary<string, string>
                {
                    { "name", name },
                    { "result", result},
                    { "type", type },
                    { "settings", settings },
                    { "hostname", "PROXY_"+Environment.MachineName }
                };
                    var content = new FormUrlEncodedContent(values);
                    using (var client = new HttpClient())
                    {
                        var response =
                            client.PostAsync("http://jakubsmiddizertaceproxy.azurewebsites.net/Home/SaveResults",
                                content);
                        response.Wait();
                        var responseResult = response.Result;
                        if (responseResult.StatusCode != HttpStatusCode.OK)
                        {
                            throw new Exception("Status code = "+responseResult.StatusCode);
                        }
                    }
                }
                catch (Exception ex)
                {

                    Logger.Error("Failed insert result using proxy service.", ex);
                }
            }
            return false;
        }

        public static string GetSettingsForComputer()
        {
            string machineName = Environment.MachineName;
            try
            {
                using (var dbProvider = DbEntitiesProviderFactory.CreateDbEntitiesProvider(false))
                {
                    var result = dbProvider.GetSettingsForHost(machineName);
                    if (string.IsNullOrEmpty(result))
                    {
                        Logger.Info("No specific settings for machine "+machineName+" found, using DEFAULT");
                        result = dbProvider.GetSettingsForHost("DEFAULT");
                    }
                    if (string.IsNullOrEmpty(result))
                    {
                        throw  new Exception("No settings found for "+machineName);
                    }
                    Logger.Info("Retrieved settings from the database");
                    return result;
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed obtained settings. Falling back for proxy service.", e);
                try
                {
                    using (var client = new HttpClient())
                    {
                        var responseString = client.GetStringAsync("http://jakubsmiddizertaceproxy.azurewebsites.net/Home/GetSettings?HostName="+machineName);
                        responseString.Wait();
                        return responseString.Result;
                    }
                }
                catch (Exception ex)
                {

                    Logger.Error("Failed obtained settings from proxy service.", ex);
                }
            }
            return "end";
        }
    }
}
