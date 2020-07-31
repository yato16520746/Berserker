using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    enum EditorMode
    {
        DrawTerrain,
        NavigationAI,
        GameOn
    }

#if UNITY_EDITOR

    [Header("Editor Mode")]
    [SerializeField] EditorMode _editorMode;

    [Space]
    [SerializeField] List<GameObject> _NavigationAI;
    [SerializeField] List<GameObject> _AIOnly;

    [Space]
    [SerializeField] GameObject _planeGround;
    [SerializeField] Terrain _terrain;

    private void OnValidate()
    {
        switch (_editorMode)
        {
            case EditorMode.DrawTerrain:
                _terrain.GetComponent<TerrainCollider>().enabled = true;
                _terrain.gameObject.SetActive(true);
                foreach (GameObject go in _NavigationAI)
                {
                    go.GetComponent<MeshRenderer>().enabled = true;
                }
                foreach (GameObject go in _AIOnly)
                {
                    go.SetActive(false);
                }
                //_planeGround.SetActive(false);
                break;

            case EditorMode.NavigationAI:
                _terrain.gameObject.SetActive(false);
                foreach (GameObject go in _NavigationAI)
                {
                    go.GetComponent<MeshRenderer>().enabled = true;
                }
                foreach (GameObject go in _AIOnly)
                {
                    go.SetActive(true);
                }
                //_planeGround.SetActive(false);
                break;

            case EditorMode.GameOn:
                _terrain.GetComponent<TerrainCollider>().enabled = false;
                foreach(GameObject go in _NavigationAI)
                {
                    go.GetComponent<MeshRenderer>().enabled = false;
                }
                foreach (GameObject go in _AIOnly)
                {
                    go.SetActive(false);
                }
                //_planeGround.SetActive(true);
                
                break;
        }
    }
#endif
}
