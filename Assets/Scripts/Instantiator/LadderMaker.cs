using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMaker : MonoBehaviour {
    public Material ShortBarMaterial;
    public Material LongBarMaterial;

    void Start()
    {
        Vector3 boxSize = this.gameObject.transform.localScale;

        GameObject ladder = new GameObject();
        Instantiate(ladder, this.transform);

        //Creating template vertical bars 
        GameObject longbarTemplate = new GameObject("Longbar");
        longbarTemplate.transform.localScale = new Vector3(boxSize.x * 0.2f, boxSize.y, boxSize.z);
        longbarTemplate.AddComponent<MeshRenderer>().material = LongBarMaterial;
        longbarTemplate.AddComponent<MeshFilter>().mesh = this.GetComponent<MeshFilter>().mesh;

        //Creating template for horizontal bars
        GameObject shortbarTemplate = new GameObject("Shortbar");
        shortbarTemplate.transform.localScale = new Vector3(boxSize.x * 0.8f, 0.4f, boxSize.z*0.8f);
        shortbarTemplate.AddComponent<MeshRenderer>().material = ShortBarMaterial;
        shortbarTemplate.AddComponent<MeshFilter>().mesh = this.GetComponent<MeshFilter>().mesh;

        //Creating template for trigger zones to enter ladder
        GameObject triggerTemplateStart = new GameObject("Lader Start Trigger");
        triggerTemplateStart.transform.localScale = new Vector3(boxSize.x * 0.6f, 1f, 2f);
        triggerTemplateStart.AddComponent<BoxCollider>().isTrigger = true;
        triggerTemplateStart.AddComponent<TriggerLadderStart>();

        //Creating template for trigger zones to exit ladder
        GameObject triggerTemplateEnd = new GameObject("Lader End Trigger");
        triggerTemplateEnd.transform.localScale = new Vector3(boxSize.x * 0.6f, 0.01f, 1f);
        triggerTemplateEnd.AddComponent<BoxCollider>().isTrigger = true;
        triggerTemplateEnd.AddComponent<TriggerLadderEnd>();

        //Instantiate longbars
        Instantiate(longbarTemplate, ladder.transform.position - Vector3.right * boxSize.x * 2f / 5f, Quaternion.identity, ladder.transform);
        Instantiate(longbarTemplate, ladder.transform.position + Vector3.right * boxSize.x * 2f / 5f, Quaternion.identity, ladder.transform);

        //Instantiate shortbars
        for(float i = -boxSize.y/2f+1f  ; i < boxSize.y/2f; i++)
        {
            Instantiate(shortbarTemplate, ladder.transform.position + Vector3.up * i, Quaternion.identity, ladder.transform);
        }

        //Instantiate triggers
        TriggerLadderStart triggerLadderStartBottom = Instantiate(triggerTemplateStart, ladder.transform.position - Vector3.forward - Vector3.up * (boxSize.y) / 2f, Quaternion.identity, ladder.transform).GetComponent<TriggerLadderStart>();
        triggerLadderStartBottom.Position = this.transform.position - Vector3.up * (boxSize.y) / 2f;
        triggerLadderStartBottom.Rotation = this.transform.rotation;
        triggerLadderStartBottom.GoingUp = true;

        TriggerLadderStart triggerLadderStartTop = Instantiate(triggerTemplateStart, ladder.transform.position + Vector3.forward + Vector3.up * (boxSize.y) / 2f, Quaternion.identity, ladder.transform).GetComponent<TriggerLadderStart>();
        triggerLadderStartTop.Position = this.transform.position + Vector3.up * (boxSize.y) / 2f;
        triggerLadderStartTop.Rotation = this.transform.rotation;
        triggerLadderStartTop.GoingUp = false;

        Instantiate(triggerTemplateEnd, ladder.transform.position - Vector3.forward + Vector3.up * (boxSize.y) / 2f, Quaternion.identity, ladder.transform);

        ladder.transform.position = this.transform.position;
        ladder.transform.rotation = this.transform.rotation;
        ladder.AddComponent<BoxCollider>().size = new Vector3(boxSize.x * 1.05f, boxSize.y, boxSize.z * 1.05f);

        //Destroy templates and placeholder
        Destroy(triggerTemplateStart);
        Destroy(triggerTemplateEnd);
        Destroy(longbarTemplate);
        Destroy(shortbarTemplate);
        Destroy(this.gameObject);
    }
}
