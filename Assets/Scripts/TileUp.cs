using UnityEditor;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;

public class TileUp : Tile
{
    [Header("- Stats -")]
    public TileUpType type;
    private TileUpType oldType;

    [ShowIf("isWinTrappe")]
    public int numberPartKeyRequired = 0;

    [ShowIf("isBrasero")]
    public int refillAmountBrasero = 10;

    [ShowIf("isTorch")]
    public int refillAmountTorch = 5;

    [ShowIf("isVentilateur")]
    public TileDown.Direction dirWind;
    private TileDown.Direction oldDirWind;
    [ShowIf("isVentilateur")]
    public TypeVentilateur typeVentilateur;


    [ShowIf("isWind")]
    public TileDown.Direction direction;
    [ShowIf("isWind")]
    public int pushNumberTiles;
    [ShowIf("isWind")]
    public DirectionLock cornerLockDir;

    [ShowIf("isBlock")]
    public GameObject block;

    [ShowIf("isWall")]
    public WallPosition wallPosition;

    [ShowIf("isWallSide")]
    public WallSideOrientation _wallSideOrientation;

    [ShowIf("isWallCorner")]
    public WallCornerOrientation _wallCornerOrientation;

    [Header("- ToHook -")]
    [SerializeField] GameObject blockPrefab;
    public GameObject lightPrefab;
    public GameObject flameTorchPrefab;
    public GameObject flameBraseroPrefab;
    public GameObject grillePrefab;
    [SerializeField] SpriteUp sprites;

    //public hide
    [HideInInspector] public bool isActivated = false;
    [HideInInspector] public TileMap tileMap;
    [HideInInspector] public TileUpMap tileUpMap;

    [HideInInspector] public bool wasWind;
    [HideInInspector] public int blockerBlock;

    [HideInInspector] public GameObject lightBrasero;
    [HideInInspector] public GameObject flameBrasero;
    [HideInInspector] public GameObject lightTorch;
    [HideInInspector] public GameObject flameTorch;
    [HideInInspector] public GameObject lightKey;
    [HideInInspector] public GameObject lightTrappe;
    [HideInInspector] public GameObject grilleTrappe;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public float moveTimer;

    public enum DirectionLock
    {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public enum TileUpType
    {
        None,
        Wall,
        Key,
        Brasero,
        Torch,
        Block,
        WinTrappe,
        Collectible,
        Ventilateur,
        Wind
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GoBackToWhite ()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<SpriteRenderer>().sprite = spritesUp.spriteNone[0];
    }

    [Button("RefreshTile")]
    private void RefreshTile()
    {
        RefreshColorSprite(false);
    }

    ////coroutine qui met et enleve le material de block
    //public IEnumerator BlockedByWall(float noPassTime, float noPassTimeAfter)
    //{
    //    moveTimer = 0;
    //    spriteRenderer.sharedMaterial = new Material(spritesUp.noPassMat);
    //    bool _hasStopped = false;

    //    while (moveTimer < noPassTime)
    //    {
    //        moveTimer += Time.fixedDeltaTime;

    //        float temp = Mathf.PingPong(moveTimer / noPassTime * 2, 1f);

    //        if (!_hasStopped && temp > 0.95f)
    //        {
    //            yield return new WaitForSeconds(noPassTimeAfter);
    //            _hasStopped = true;
    //        }

    //        spriteRenderer.sharedMaterial.SetFloat("_TimeControlled", temp);

    //        yield return new WaitForFixedUpdate();
    //    }

    //    Destroy(spriteRenderer.sharedMaterial);

    //    spriteRenderer.sharedMaterial = spritesUp.normalMat;
    //    moveTimer = 0;
    //}

#if (UNITY_EDITOR)
    // change la door lorqu'on la met dans l'inspecteur
    private void OnValidate()
    {
        // si l'on ne d�signe plus la case comme �tant block, on delete le block (object)
        if (type != TileUpType.Block && block != null)
        {
            EditorApplication.delayCall += () =>
            {
                DestroyImmediate(block);
            };
        }

        if (oldType != type)
        {
            RefreshColorSprite(true);

            switch (oldType)
            {
                case TileUpType.Wall:
                    if(GetComponent<ShadowCaster2D>() != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(GetComponent<ShadowCaster2D>());
                        };
                    }
                    
                    break;

                case TileUpType.Ventilateur:
                    if (GetComponent<ShadowCaster2D>() != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(GetComponent<ShadowCaster2D>());
                        };
                    }

                    
                    Vector3 nextPos = transform.position + DirectionAddMovePos(dirWind);
                    RecursiveCheckNextWind(nextPos, dirWind, true, spritesUp.spriteNone[0]);
                    oldType = type;
                    break;

                case TileUpType.Torch:
                    if (lightTorch != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(lightTorch);
                        };
                    }
                    if (flameTorch != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(flameTorch);
                        };
                    }
                    break;

                case TileUpType.Brasero:
                    if (lightBrasero != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(lightBrasero);
                        };
                    }
                    if (flameBrasero != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(flameBrasero);
                        };
                    }
                    break;

                case TileUpType.Key:
                    GetComponent<SpriteRenderer>().material = spritesUp.normalMat;

                    if (lightKey != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(lightKey);
                        };
                    }
                    break;
                case TileUpType.WinTrappe:
                    if (lightTrappe != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(lightTrappe);
                        };
                    }
                    if(grilleTrappe != null)
                    {
                        UnityEditor.EditorApplication.delayCall += () =>
                        {
                            DestroyImmediate(grilleTrappe);
                        };
                    }
                    break;

                case TileUpType.Collectible:
                    GetComponent<SpriteRenderer>().material = spritesUp.normalMat;
                    break;

                default:
                    break;
            }
        }

        if (oldDirWind != dirWind && type == TileUpType.Ventilateur)   
        {
            Vector3 nextPosSuppr = transform.position + DirectionAddMovePos(oldDirWind);
            RecursiveCheckNextWind(nextPosSuppr, oldDirWind, true, spritesUp.spriteNone[0]);

            Vector3 nextPosAdd = transform.position + DirectionAddMovePos(dirWind);
            RecursiveCheckNextWind(nextPosAdd, dirWind, false, spritesUp.spriteWind[0]);

            oldDirWind = dirWind;
        }

        oldType = type;
    }
    #endif

    public void RefreshColorSprite(bool checkBlock)
    {
        switch (type)
        {
            case TileUpType.None:
                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteNone[0];
                
                break;
            case TileUpType.Wall:
                GetComponent<SpriteRenderer>().color = Color.white;
                switch (wallPosition)
                {
                    case WallPosition.None:
                        GetComponent<SpriteRenderer>().sprite = spritesUp.spriteNoneWall[0];
                        break;
                    case WallPosition.Side:
                        switch (_wallSideOrientation)
                        {
                            case WallSideOrientation.UpOneSide:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[Random.Range(0, 2)];
                                break;
                            case WallSideOrientation.DownOneSide:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[2];
                                break;
                            case WallSideOrientation.LeftOneSide:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[Random.Range(3, 5)];
                                break;
                            case WallSideOrientation.RightOneSide:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[Random.Range(5, 7)];
                                break;
                            case WallSideOrientation.VerticalTwoSides:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[Random.Range(7, 9)];
                                break;
                            case WallSideOrientation.HorizontalTwoSides:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[9];
                                break;
                            case WallSideOrientation.EndHorizontalTwoSidesR:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[10];
                                break;
                            case WallSideOrientation.EndHorizontalTwoSidesL:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[11];
                                break;
                            case WallSideOrientation.EndVerticalTwoSidesU:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[12];
                                break;
                            case WallSideOrientation.EndVerticalTwoSidesD:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[13];
                                break;
                            case WallSideOrientation.SoloSide:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteSideWall[14];
                                break;
                        }
                        break;
                    case WallPosition.Corner:

                        switch (_wallCornerOrientation)
                        {
                            case WallCornerOrientation.LeftDownExterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[0];
                                break;
                            case WallCornerOrientation.LeftUpExterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[1];
                                break;
                            case WallCornerOrientation.RightDownExterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[2];
                                break;
                            case WallCornerOrientation.RightUpExterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[3];
                                break;
                            case WallCornerOrientation.LeftDownInterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[4];
                                break;
                            case WallCornerOrientation.LeftUpInterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[5];
                                break;
                            case WallCornerOrientation.RightDownInterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[6];
                                break;
                            case WallCornerOrientation.RightUpInterior:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[7];
                                break;
                            case WallCornerOrientation.LeftDownDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[8];
                                break;
                            case WallCornerOrientation.RightDownDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[9];
                                break;
                            case WallCornerOrientation.LeftUpDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[10];
                                break;
                            case WallCornerOrientation.RightUpDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[11];
                                break;
                            case WallCornerOrientation.TUpDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[12];
                                break;
                            case WallCornerOrientation.TDownDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[13];
                                break;
                            case WallCornerOrientation.TLeftDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[14];
                                break;
                            case WallCornerOrientation.TRightDouble:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[15];
                                break;
                            case WallCornerOrientation.CornerRightUpLeftDown:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[16];
                                break;
                            case WallCornerOrientation.CornerRightDownLeftUp:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[17];
                                break;
                            case WallCornerOrientation.CornerRightDownRightUp:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[18];
                                break;
                            case WallCornerOrientation.CornerLeftDownLeftUp:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[19];
                                break;
                            case WallCornerOrientation.CornerRightDownLeftDown:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[20];
                                break;
                            case WallCornerOrientation.CornerRightUpLeftUp:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[21];
                                break;
                            case WallCornerOrientation.LeftDoubleDownSoloUp:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[22];
                                break;
                            case WallCornerOrientation.LeftDoubleUpSoloDown:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[23];
                                break;
                            case WallCornerOrientation.RightDoubleDownSoloUp:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[24];
                                break;
                            case WallCornerOrientation.RightDoubleUpSoloDown:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[25];
                                break;
                            case WallCornerOrientation.UpDoubleLeftSoloRight:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[26];
                                break;
                            case WallCornerOrientation.UpDoubleRightSoloLeft:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[27];
                                break;
                            case WallCornerOrientation.DownDoubleLeftSoloRight:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[28];
                                break;
                            case WallCornerOrientation.DownDoubleRightSoloLeft:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[29];
                                break;
                            case WallCornerOrientation.FourDirections:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCornerWall[30];
                                break;

                        }
                        break;
                }
                if(GetComponent<ShadowCaster2D>() == null)
                {
                    gameObject.AddComponent<ShadowCaster2D>();
                }
                
                ShadowCaster2D shadowCastTemp = GetComponent<ShadowCaster2D>();
                shadowCastTemp.useRendererSilhouette = false;
                shadowCastTemp.selfShadows = true;
                break;

            case TileUpType.Key:
                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteKey[0];
                GetComponent<SpriteRenderer>().sharedMaterial = new Material(spritesUp.collectibleMat);

                if (lightKey == null)
                {
                    GameObject tempLightK = Instantiate(lightPrefab, transform);
                    lightKey = tempLightK;

                }
                lightKey.GetComponent<Light2D>().pointLightOuterRadius = sprites.radiusLightKey;
                lightKey.GetComponent<Light2D>().color = sprites.colorLightKey;
                break;


            case TileUpType.Torch:
                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteTorch[0];
                if (lightTorch == null)
                {
                    GameObject tempLightT = Instantiate(lightPrefab, transform);
                    lightTorch = tempLightT;
                    GameObject tempFlameT = Instantiate(flameTorchPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity, transform);
                    flameTorch = tempFlameT;

                    
                }
                flameTorch.transform.localPosition = new Vector3(0, 0.3f, 0);
                lightTorch.GetComponent<Light2D>().pointLightOuterRadius = sprites.radiusLightTorch;
                lightTorch.GetComponent<Light2D>().color = sprites.colorLightTorch;
                break;

            case TileUpType.Brasero:
                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteBrasero[0];
                if (lightBrasero == null)
                {
                    GameObject tempLightB = Instantiate(lightPrefab, transform);
                    lightBrasero = tempLightB;
                    GameObject tempFlameB = Instantiate(flameBraseroPrefab, transform.position + new Vector3(0, 0.527f, 0), Quaternion.identity, transform);
                    flameBrasero = tempFlameB;                
                    
                }
                flameBrasero.transform.localPosition = new Vector3(0, 0.527f, 0);
                lightBrasero.GetComponent<Light2D>().pointLightOuterRadius = sprites.radiusLightBrasero;
                lightBrasero.GetComponent<Light2D>().color = sprites.colorLightBrasero;

                break;


            case TileUpType.WinTrappe:
                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteWinTrappe[0];
                if (lightTrappe == null)
                {
                    GameObject tempLightW = Instantiate(lightPrefab, transform);
                    lightTrappe = tempLightW;

                }
                lightTrappe.GetComponent<Light2D>().pointLightOuterRadius = sprites.radiusLightTrappe;
                lightTrappe.GetComponent<Light2D>().color = sprites.colorLightTrappe;
                if(grilleTrappe == null && grillePrefab != null)
                {
                    GameObject tempGrille= Instantiate(grillePrefab, transform);
                    grilleTrappe = tempGrille;
                }
                break;

            case TileUpType.Ventilateur:
                Vector3 nextPos = transform.position + DirectionAddMovePos(dirWind);
                RecursiveCheckNextWind(nextPos, dirWind, false, spritesUp.spriteWind[0]);
                switch (typeVentilateur)
                {
                    case TypeVentilateur.Block:
                        switch (dirWind)
                        {
                            case TileDown.Direction.Left:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[1];
                                break;
                            case TileDown.Direction.Right:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[2];
                                break;
                            case TileDown.Direction.Up:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[3];
                                break;
                            case TileDown.Direction.Down:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[0];
                                break;
                            default:
                                break;
                        }
                        break;
                    case TypeVentilateur.Wall:
                        switch (dirWind)
                        {
                            case TileDown.Direction.Left:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[5];
                                break;
                            case TileDown.Direction.Right:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[6];
                                break;
                            case TileDown.Direction.Up:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[7];
                                break;
                            case TileDown.Direction.Down:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[4];
                                break;
                            default:
                                break;
                        }
                        break;
                    case TypeVentilateur.DoubleWall:
                        switch (dirWind)
                        {
                            case TileDown.Direction.Left:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[9];
                                break;
                            case TileDown.Direction.Right:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[10];
                                break;
                            case TileDown.Direction.Up:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[11];
                                break;
                            case TileDown.Direction.Down:
                                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteVentilateur[8];
                                break;
                            default:
                                break;
                        }
                        break;
                }

                if (GetComponent<ShadowCaster2D>() == null)
                {
                    gameObject.AddComponent<ShadowCaster2D>();
                }

                ShadowCaster2D shadowCastTemp2 = GetComponent<ShadowCaster2D>();
                shadowCastTemp2.useRendererSilhouette = false;
                shadowCastTemp2.selfShadows = true;

                break;

            case TileUpType.Wind:
                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteWind[Random.Range(0, spritesUp.spriteWind.Count)];
                break;

            case TileUpType.Block:
                if (block == null && checkBlock)
                {
                    GameObject temp = Instantiate(blockPrefab, transform);
                    block = temp;
                }
                block.GetComponent<BlockToMove>().spritesDown = spritesDown;

                break;
                
            case TileUpType.Collectible:
                GetComponent<SpriteRenderer>().sharedMaterial = new Material(spritesUp.collectibleMat);
                GetComponent<SpriteRenderer>().sprite = spritesUp.spriteCollectible[0];
                break;

        }
    }

    private void RecursiveCheckNextWind(Vector3 pos, TileDown.Direction direction, bool isPutting, Sprite spriteReplace)
    {
        //TileDown tempTileDown = tileMap.FindTileWithPosEditor(pos);
        TileUp tempTileUp = tileUpMap.FindTileWithPosEditor(pos);

        pos += DirectionAddMovePos(direction);

        if (tempTileUp == null || tempTileUp.type == TileUpType.WinTrappe || tempTileUp.type == TileUpType.Wall || tempTileUp.type == TileUpType.Ventilateur || tempTileUp.type == TileUpType.Block || tempTileUp.type == TileUpType.Torch || tempTileUp.type == TileUpType.Brasero)
        {
            if (tempTileUp != null)
            {
                tempTileUp.wasWind = true;
                tempTileUp.isActivated = false;
            }
            return;
        }

        else if (!isPutting)
        {
            tempTileUp.type = TileUpType.Wind;

            if (tempTileUp.cornerLockDir == DirectionLock.None)
            {
                tempTileUp.direction = direction;
            }
            else
            {
                tempTileUp.direction = (TileDown.Direction)(tempTileUp.cornerLockDir - 1);
            }
            
            tempTileUp.pushNumberTiles = 1;
            tempTileUp.GetComponent<SpriteRenderer>().sprite = spriteReplace;
            tempTileUp.GetComponent<SpriteRenderer>().material = spritesUp.windMat[(int) tempTileUp.direction];

            //RotateDirectionWind(tempTileUp, tempTileUp.direction);

            RecursiveCheckNextWind(pos, direction, isPutting, spriteReplace);
        }
        else
        {
            tempTileUp.type = TileUpType.None;
            tempTileUp.GetComponent<SpriteRenderer>().sprite = spriteReplace;
            tempTileUp.GetComponent<SpriteRenderer>().material = spritesUp.normalMat;
            tempTileUp.transform.rotation = Quaternion.Euler(0, 0, 0);

            RecursiveCheckNextWind(pos, direction, isPutting, spriteReplace);
        }
    }

    public void RotateDirectionWind (TileUp tempTileUp, TileDown.Direction direction)
    {
        switch (direction)
        {
            case TileDown.Direction.Left:
                tempTileUp.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case TileDown.Direction.Right:
                tempTileUp.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case TileDown.Direction.Up:
                tempTileUp.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case TileDown.Direction.Down:
                tempTileUp.transform.rotation = Quaternion.Euler(0, 0, -180);
                break;
        }
    }

    public void PutOrWithdrawShaderWind (bool isStopped)
    {
        if (!isStopped)
        {
            GetComponent<SpriteRenderer>().material = spritesUp.windMat[(int) direction];
        }
        else
        {
            GetComponent<SpriteRenderer>().material = spritesUp.normalMat;
        }
    }

    

    public void SwitchOffTorch()
    {
        DOTween.To(() => lightTorch.GetComponent<Light2D>().pointLightOuterRadius, x => lightTorch.GetComponent<Light2D>().pointLightOuterRadius = x, 0, 0.5f).SetEase(Ease.OutExpo);
        Destroy(flameTorch);
        Destroy(lightTorch);
    }
    public enum WallPosition
    {
        None,
        Side,
        Corner
    }

    public enum TypeVentilateur
    {
        Block,
        Wall,
        DoubleWall
    }

    private bool isWall() { return type == TileUpType.Wall; }
    private bool isBrasero() { return type == TileUpType.Brasero; }
    private bool isTorch() { return type == TileUpType.Torch; }
    private bool isBlock() { return type == TileUpType.Block; }
    private bool isWinTrappe() { return type == TileUpType.WinTrappe; }
    private bool isWallSide() { return wallPosition == WallPosition.Side; }
    private bool isWallCorner() { return wallPosition == WallPosition.Corner; }
    private bool isVentilateur() { return type == TileUpType.Ventilateur; }
    private bool isWind() { return type == TileUpType.Wind; }

    public enum WallCornerOrientation
    {
        LeftUpExterior,
        RightUpExterior,
        LeftDownExterior,
        RightDownExterior,
        LeftUpInterior,
        RightUpInterior,
        LeftDownInterior,
        RightDownInterior,
        LeftDownDouble,
        RightDownDouble,
        LeftUpDouble,
        RightUpDouble,
        TUpDouble,
        TDownDouble,
        TLeftDouble,
        TRightDouble,
        CornerRightUpLeftDown,
        CornerRightDownLeftUp,
        CornerRightDownRightUp,
        CornerLeftDownLeftUp,
        CornerRightDownLeftDown,
        CornerRightUpLeftUp,
        LeftDoubleUpSoloDown,
        LeftDoubleDownSoloUp,
        RightDoubleUpSoloDown,
        RightDoubleDownSoloUp,
        UpDoubleLeftSoloRight,
        UpDoubleRightSoloLeft,
        DownDoubleLeftSoloRight,
        DownDoubleRightSoloLeft,
        FourDirections
    }

    public enum WallSideOrientation
    {
        LeftOneSide,
        RightOneSide,
        DownOneSide,
        UpOneSide,
        HorizontalTwoSides,
        VerticalTwoSides,
        EndHorizontalTwoSidesL,
        EndHorizontalTwoSidesR,
        EndVerticalTwoSidesU,
        EndVerticalTwoSidesD,
        SoloSide
    }


}
