using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GachaController : MonoBehaviour
{
    public List<GameObject> vouchers;
    public List<GameObject> vouchers2;
    public Transform gacha;
    public GameObject textVoucherPrefab;
    public List<TextMeshProUGUI> text;
    public Transform point;
    public GameObject notification;
    public GameObject gameOver;

    [SerializeField] private float speedStart = 10f;
    private float lastSpeed;
    private bool rotate;
    private float reduceVelocity;
    private int round = -1;
    private GameObject selectedVoucher;

    private void Awake()
    {
        vouchers = new List<GameObject>();
        vouchers2 = new List<GameObject>();
    }
    void Start()
    {
        CreateVoucher();
        StartCoroutine(Notification());
    }

    void Update()
    {
        PressButton();
        if (rotate)
        {
            RotateGacha();
        }
    }

    private void CreateVoucher()
    {
        lastSpeed = speedStart;
        for (int i = 0; i < 12; i++)
        {
            GameObject newVoucher = Instantiate(textVoucherPrefab, gacha);

            Quaternion gachaRotation = gacha.transform.rotation;
            Quaternion additionalRotation = Quaternion.Euler(-90, 30 * i + 5, 0);
            Quaternion newRotation = gachaRotation * additionalRotation;

            newVoucher.transform.rotation = newRotation;

            Vector3 offset = new Vector3(0, -1.8f, -0.8f);
            Vector3 newPosition = gacha.position + newRotation * offset;
            newVoucher.transform.position = newPosition;

            newVoucher.GetComponent<TextMeshPro>().text = "discount " + i * 2 + " % ";
            newVoucher.transform.localScale = new Vector3(-0.01f, -0.01f, 0.01f);

            vouchers.Add(newVoucher);
        }
        for (int i = 0; i < 12; i++)
        {
            GameObject newVoucher = Instantiate(textVoucherPrefab, gacha);

            Quaternion gachaRotation = gacha.transform.rotation;
            Quaternion additionalRotation = Quaternion.Euler(0, 30 * i - 80, 0);
            Quaternion newRotation = gachaRotation * additionalRotation;
            Quaternion editRotation = Quaternion.Euler(0, -83, 0);

            newVoucher.transform.rotation = newRotation * editRotation;

            Vector3 offset = new Vector3(2.45f, 0, 0);
            Vector3 newPosition = gacha.position + newRotation * offset;
            newVoucher.transform.position = newPosition;

            newVoucher.GetComponent<TextMeshPro>().text = "discount " + i * 2 + " % ";

            vouchers2.Add(newVoucher);
        }
    }

    private void RotateGacha()
    {
        gacha.Rotate(0, lastSpeed, 0);

        if (lastSpeed > 1.5f)
        {
            lastSpeed -= speedStart / reduceVelocity * Time.deltaTime;
        }
        else if (lastSpeed <= 1.5f && lastSpeed > 0)
        {
            float voucherRotationZ = selectedVoucher.transform.rotation.eulerAngles.z;

            if (voucherRotationZ > 65 && voucherRotationZ < 95)
            {
                lastSpeed = 0;
                rotate = false;
                int turn = round + 1;
                text[round].text = "\tTurn " + turn + " : " + selectedVoucher.GetComponent<TextMeshPro>().text;
            }
            else
            {
                lastSpeed -= speedStart / 30 * Time.deltaTime;
            }
        }
    }

    private void PressButton()
    {
        if (Input.GetMouseButtonDown(0) && !rotate && round < 7)
        {
            round++;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "Play")
                {
                    selectedVoucher = RandomResult();
                    Debug.Log(selectedVoucher.GetComponent<TextMeshPro>().text);
                    rotate = true;
                    lastSpeed = speedStart;
                    reduceVelocity = Random.Range(4, 6);
                }
            }
        }
    }

    private GameObject RandomResult()
    {
        var voucherRandom = vouchers[Random.Range(0, vouchers.Count)];
        return voucherRandom;
    }

    IEnumerator Notification()
    {
        notification.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        notification.SetActive(false);
    }
}
