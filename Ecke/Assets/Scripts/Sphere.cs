using System.Collections;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    private const float Velocity = 5f;
    private Vector3 _lastFrameVelocity;
    private Rigidbody _rigidbody;
    private bool _startMovement = true;
    private Vector3 _initialPosition;
    private Vector2 _screenSize;

    // Start is called before the first frame update
    private void Start()
    {
        _screenSize = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0);
        _initialPosition = _rigidbody.position;
    }

    // Update is called once per frame
    private void Update()
    {
        _lastFrameVelocity = _rigidbody.velocity;
        if (DrawLine.currLines == 1 && _startMovement)
        {
            _rigidbody.velocity = new Vector3(0, -10f, 0);
            _startMovement = false;
            GetComponent<TrailRenderer>().enabled = true;
        }

        DestroyLine();
        Restart();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var contactPoint in collision.contacts)
        {
            _rigidbody.velocity = Vector3.Reflect(_lastFrameVelocity.normalized, contactPoint.normal) *
                                  Mathf.Max(Velocity, _lastFrameVelocity.magnitude);
            return;
        }
    }

    void Restart() //in DrawLine neustart Logik die zu DrawLine gehört reinmachen.
    {
        if (CheckOffScreen(_rigidbody.position, _screenSize) && !Target.LevelDone)
        {
            _rigidbody.position = _initialPosition;
            _rigidbody.velocity = Vector3.zero;
            _startMovement = true;
            var matSphere = GetComponent<Renderer>().material;
            var colorSphere = matSphere.color;
            colorSphere.a = 0;
            matSphere.color = colorSphere;
            StartCoroutine(Animations.FadeTo(matSphere, 1f, 0.3f));
            GetComponent<TrailRenderer>().enabled = false;

            if (CheckIfLineExists())
            {
                var matLine = GameObject.Find("Line0").GetComponent<Renderer>().material;
                StartCoroutine(Animations.FadeTo(matLine, 0f, 0.3f));
            }

            DrawLine.currLines = 0;
        }
    }

    bool CheckOffScreen(Vector2 pos, Vector2 screenSize)
    {
        return (pos.x < -screenSize.x || pos.x > screenSize.x || pos.y < -screenSize.y || pos.y > screenSize.y);
    }

    private bool CheckIfLineExists()
    {
        return GameObject.Find("Line0") != null;
    }

    private void DestroyLine()
    {
        if (CheckIfLineExists())
        {
            if (GameObject.Find("Line0").GetComponent<Renderer>().material.color.a < 0.1f)
            {
                Destroy(GameObject.Find("Line0"));
            }
        }
    }

    
}