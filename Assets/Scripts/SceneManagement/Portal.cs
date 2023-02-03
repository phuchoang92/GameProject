using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPG.SceneManagement;

public class Portal : MonoBehaviour
{
    enum DestinationIdentifier
    {
        A, B, C, D, E
    }

    [SerializeField] int sceneToLoad = -1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destination;
    [SerializeField] float FadeOutTime = 1f;
    [SerializeField] float FadeInTime = 2f;
    [SerializeField] float FadeWaitTime = 0.5f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("1");
            StartCoroutine(Transition());
        }   
    }
    private IEnumerator Transition()
    {
        if(sceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }


        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();

        yield return fader.FadeOut(FadeOutTime);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);

        yield return new WaitForSeconds(FadeWaitTime);
        yield return fader.FadeIn(FadeInTime);

        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.destination != destination) continue;

            return portal;
        }
        return null;
    }
}
