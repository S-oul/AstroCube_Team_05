using UnityEngine;

/// <summary>
/// Gère l'activation et la désactivation séquentielle de trigger boxes.
/// Quand le joueur entre dans cette trigger (la deuxième), elle active la suivante et désactive la précédente.
/// </summary>
public class AUDIO_TriggerLoader : MonoBehaviour
{
    [Header("Trigger Box References")]
    [Tooltip("La trigger box précédente à désactiver")]
    [SerializeField] private GameObject previousTrigger;
    
    [Tooltip("La trigger box suivante à activer")]
    [SerializeField] private GameObject nextTrigger;
    
    [Header("Settings")]
    [Tooltip("Tag du joueur pour détecter l'entrée dans la trigger")]
    [SerializeField] private string playerTag = "Player";
    
    [Tooltip("Désactiver ce GameObject après activation ?")]
    [SerializeField] private bool disableSelfAfterTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si c'est le joueur qui entre dans la trigger
        if (other.CompareTag(playerTag))
        {
            ActivateSequence();
        }
    }

    /// <summary>
    /// Active la trigger suivante et désactive la précédente
    /// </summary>
    private void ActivateSequence()
    {
        // Désactiver la trigger précédente si elle est assignée
        if (previousTrigger != null)
        {
            previousTrigger.SetActive(false);
            Debug.Log($"[AUDIO_TriggerLoader] Trigger précédente désactivée: {previousTrigger.name}");
        }
        else
        {
            Debug.LogWarning("[AUDIO_TriggerLoader] Aucune trigger précédente assignée !");
        }

        // Activer la trigger suivante si elle est assignée
        if (nextTrigger != null)
        {
            nextTrigger.SetActive(true);
            Debug.Log($"[AUDIO_TriggerLoader] Trigger suivante activée: {nextTrigger.name}");
        }
        else
        {
            Debug.LogWarning("[AUDIO_TriggerLoader] Aucune trigger suivante assignée !");
        }

        // Optionnel: désactiver ce GameObject après l'activation
        if (disableSelfAfterTrigger)
        {
            gameObject.SetActive(false);
            Debug.Log($"[AUDIO_TriggerLoader] Ce trigger a été désactivé: {gameObject.name}");
        }
    }

    // Méthode pour tester manuellement depuis l'inspecteur ou d'autres scripts
    public void ManualActivate()
    {
        ActivateSequence();
    }

#if UNITY_EDITOR
    // Visualisation dans l'éditeur pour faciliter le setup
    private void OnDrawGizmos()
    {
        if (previousTrigger != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, previousTrigger.transform.position);
        }

        if (nextTrigger != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, nextTrigger.transform.position);
        }
    }
#endif
}
