using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//A PlayerPlanetResponseHandler to deal with planet responses from the server
public class UpdatePopulationResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.Population; }
    }
    public UpdatePopulationResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        view.LogDebug("GOT A RESPONSE for KNOWN Planets");
        if (response.ReturnCode == 0)
        {
            // TODO FIX THIS FOR ONLY 1 PLANET??
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Planets].ToString());

            // Deserialize
            var xmlData = response.Parameters[(byte)ClientParameterCode.Planets].ToString();
            XmlSerializer deserializer = new XmlSerializer(typeof(XmlPlanetList));
            TextReader reader = new StringReader(xmlData);
            object obj = deserializer.Deserialize(reader);
            XmlPlanetList planetCollection = (XmlPlanetList)obj;
            reader.Close();

            // Update local data
            foreach (SanPlanet p in planetCollection.Planets) {
                var planet = PlayerData.instance.ownedPlanets.Find(x => x.starID == p.StarId && x.planetID == p.PlanetNum);
                if (planet != null)
                {
                    planet.planetpopulation = p.Population;
                }
            }              
                


        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
        }
    }
}
