using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterBodyManager : MonoBehaviour
{

    public CharacterBody characterBody;

    private Character character;

    public void ChangeCharacterBody(string bodyName)
    {
        if (!CharacterBodyBank.TryGetBody(bodyName, out var body))
            return;

        // delete old body if exists
        if (characterBody) 
            Destroy(characterBody);

        SpawnCharacterBody(body);
    }

    private void SpawnCharacterBody(GameObject body)
    {
        // because this class doesnt have a start/awake/onenable..
        if (!character) 
            character = this.transform.root.GetComponent<Character>();

        var _b = Instantiate(body, transform.position, Quaternion.identity, this.transform);
        _b.GetComponent<CharacterBody>().SetupBody();
        characterBody = _b.GetComponent<CharacterBody>();

        // change the animator
        character.characterAnimator.SetNewAnimator(_b.GetComponent<Animator>());
    }
    
    public Animator GetBodyAnimator()
    {
        return characterBody.animator;
    }

    public CharacterEffects GetBodyEffects()
    {
        return characterBody.characterEffects;
    }
    
}
