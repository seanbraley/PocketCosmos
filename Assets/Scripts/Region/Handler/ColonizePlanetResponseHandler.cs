using Completed;
using ExitGames.Client.Photon;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//A PlayerPlanetResponseHandler to deal with planet responses from the server
public class ColonizePlanetResponseHandler : PhotonOperationHandler
{
    public override byte Code
    {
        get { return (byte)MessageSubCode.ColonizePlanet; }
    }
    public ColonizePlanetResponseHandler(NetworkController controller) : base(controller)
    {
    }
    public override void OnHandleResponse(OperationResponse response)
    {
        NetworkManager view = _controller.ControlledView as NetworkManager;
        if (response.ReturnCode == 0)
        {
            // TODO FIX THIS FOR ONLY 1 PLANET??
            view.LogDebug(response.Parameters[(byte)ClientParameterCode.Planets].ToString());
            DisplayManager.Instance.DisplayMessage("Planet Colonized!");

            // Deserialize
            var xmlData = response.Parameters[(byte)ClientParameterCode.Planets].ToString();
            XmlSerializer deserializer = new XmlSerializer(typeof(XmlPlanetList));
            TextReader reader = new StringReader(xmlData);
            object obj = deserializer.Deserialize(reader);
            XmlPlanetList planetCollection = (XmlPlanetList)obj;
            reader.Close();

            // Update local data
            foreach (SanPlanet p in planetCollection.Planets)
                PlayerData.instance.AddOwnedPlanet(new OwnedPlanet(p));
        }
        else
        {
            view.LogDebug("RESPONSE: " + response.DebugMessage);
            DisplayManager.Instance.DisplayMessage(response.DebugMessage);
        }
    }
}
