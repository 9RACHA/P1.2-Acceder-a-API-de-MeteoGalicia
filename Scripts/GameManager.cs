using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    public ListaCamaras lCamaras; //lCamaras

    public RawImage imagenBruto;

    //O Awake
    void Start() {
        StartCoroutine(GetRequest("https://servizos.meteogalicia.gal/mgrss/observacion/jsonCamaras.action"));
        
        // public string url = "https://servizos.meteogalicia.gal/mgrss/observacion/jsonCamaras.action";
    }

    IEnumerator GetRequest(string uri) {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                Debug.Log("Error");
                break;
                case UnityWebRequest.Result.Success:
                CargarImagen(webRequest.downloadHandler.text);
                Debug.Log("Success");
                break;

               // default:
            }
        }

    }

    //PROBAR PUBLIC
    private void CargarJson(string jsonText) {
        lCamaras = JsonUtility.FromJson<ListaCamaras>(jsonText);
        
        if (lCamaras.listaCamaras.Count > 0) {
            int imagenSeleccionada = Random.Range(0, lCamaras.listaCamaras.Count);
            StartCoroutine(CargarImagen(lCamaras.listaCamaras[imagenSeleccionada].imaxeCamara));
        }       
    }
         
      private IEnumerator CargarImagen(string uri) {
        WWW www = new WWW(uri);
        yield return www;
        imagenBruto.texture = www.texture;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = www.texture;
        }

    }
