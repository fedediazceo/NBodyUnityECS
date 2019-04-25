//#region Copyright

//// Copyright 2014 Michael A. R. Duncan
//// You are free to do whatever you want with this source
//// File: Galaxy1Controller.cs

//#endregion

//#region

//using Assets.MickyD.Scripts;
//using UnityEngine;

//#endregion

//public class Galaxy1Controller : MonoBehaviour {
//    private const int GroupSize = 128;
//    private const int QuadStride = 12;

//    #region Fields

//    /// <summary>
//    ///     The galaxy radius
//    /// </summary>
//    /// <remarks>This will appear as a Property Drawer in Unity 4</remarks>
//    [Range(10, 1000)]
//    public float GalaxyRadius = 200;

//    public Texture2D HueTexture;

//    public int NumStars = 10000;

//    public ComputeShader StarCompute;
//    public Material StarMaterial;

//    private GameManager _manager;
//    private ComputeBuffer _quadPoints;
//    private Star[] _stars;
//    private ComputeBuffer _starsBuffer;
//    private int _updateParticlesKernel;

//    #endregion

//    // Use this for initialization

//    #region Properties

//    private Vector3 StartPointA {
//        get { return new Vector3(GalaxyRadius, 0, 0); }
//    }

//    private Vector3 StartPointB {
//        get { return new Vector3(-GalaxyRadius, 0, 0); }
//    }

//    #endregion

//    #region Methods

//    private void CreateStars(int offset, int count, Vector3 T, Vector3 V) {
//        for (var i = offset; i < offset + count; i++) {
//            var star = _stars[i];
//            star.color = Vector3.one; // white
//            star.position = Random.insideUnitSphere * GalaxyRadius + T;
//            star.velocity = V;

//            _stars[i] = star;
//        }
//    }

//    private void OnDestroy() {
//        // must deallocate here
//        _starsBuffer.Release();
//        _quadPoints.Release();
//    }

//    private void OnDrawGizmos() {
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(transform.position, GalaxyRadius);
//        Gizmos.DrawWireSphere(transform.position + StartPointA, GalaxyRadius);
//        Gizmos.DrawWireSphere(transform.position + StartPointB, GalaxyRadius);
//    }

//    private void OnRenderObject() {
//        if (!SystemInfo.supportsComputeShaders) {
//            return;
//        }

//        // bind resources to material
//        StarMaterial.SetBuffer("stars", _starsBuffer);
//        StarMaterial.SetBuffer("quadPoints", _quadPoints);

//        // set the pass
//        StarMaterial.SetPass(0);

//        // draw
//        Graphics.DrawProcedural(MeshTopology.Triangles, 6, NumStars);
//    }

//    private void Start() {
//        _updateParticlesKernel = StarCompute.FindKernel("UpdateStars");
//        if (_updateParticlesKernel == -1) {
//            Debug.LogError("Failed to find UpdateStars kernel");
//            Application.Quit();
//        }

//        _starsBuffer = new ComputeBuffer(NumStars, Constants.StarsStride);

//        _stars = new Star[NumStars];
//        var n = NumStars / 2;
//        var offset = 0;
//        CreateStars(offset, n, StartPointA, new Vector3(-10, 5, 0));
//        offset += n;

//        CreateStars(offset, n, StartPointB, new Vector3(10, -5, 0));

//        _starsBuffer.SetData(_stars);

//        _quadPoints = new ComputeBuffer(6, QuadStride);
//        _quadPoints.SetData(new[]
//        {
//            new Vector3(-0.5f, 0.5f),
//            new Vector3(0.5f, 0.5f),
//            new Vector3(0.5f, -0.5f),
//            new Vector3(0.5f, -0.5f),
//            new Vector3(-0.5f, -0.5f),
//            new Vector3(-0.5f, 0.5f),
//        });

//        _manager = FindObjectOfType<GameManager>();
//    }

//    private void Update() {
//        // bind resources to compute shader
//        StarCompute.SetBuffer(_updateParticlesKernel, "stars", _starsBuffer);
//        StarCompute.SetFloat("deltaTime", Time.deltaTime * _manager.MasterSpeed);
//        StarCompute.SetTexture(_updateParticlesKernel, "hueTexture", HueTexture);

//        // dispatch, launch threads on GPU
//        var numberOfGroups = Mathf.CeilToInt((float)NumStars / GroupSize);
//        StarCompute.Dispatch(_updateParticlesKernel, numberOfGroups, 1, 1);
//    }

//    #endregion
//}