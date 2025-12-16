using DCGO.CardEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Effects))]
[RequireComponent(typeof(ContinuousController))]
[RequireComponent(typeof(SelectPermanentEffect))]
[RequireComponent(typeof(SelectCardEffect))]
[RequireComponent(typeof(SelectHandEffect))]
[RequireComponent(typeof(SelectDigiXrosClass))]
[RequireComponent(typeof(SelectAssemblyClass))]
[RequireComponent(typeof(SelectDNACondition))]
public class GManager : MonoBehaviour 
{
    public static GManager instance = null;

    public Player You = new Player(true);
    public Player Opponent = new Player(false);

    public CommandText commandText = new CommandText();

    public AutoProcessing autoProcessing = new AutoProcessing();
    public AutoProcessing autoProcessing_CutIn = new AutoProcessing();
    public AttackProcess attackProcess = new AttackProcess();

    public MemoryObject memoryObject = new MemoryObject();

    public SelectCommandPanel selectCommandPanel = new SelectCommandPanel();
    public SelectCardPanel selectCardPanel = new SelectCardPanel();
    
    
    public SelectJogressEffect selectJogressEffect = new SelectJogressEffect();
    public SelectDNACondition selectDNACondition;
    public SelectBurstDigivolutionEffect selectBurstDigivolutionEffect = new SelectBurstDigivolutionEffect();
    public SelectAppFusionEffect selectAppFusionEffect = new SelectAppFusionEffect();
    
    public UserSelectionManager userSelectionManager = new UserSelectionManager();

    public bool isAuto;

    public PhotonWaitController photonWaitController = new PhotonWaitController();
    public TurnStateMachine turnStateMachine { get; set; }
    public bool IsAI { get; private set; } = false;
    public int CardIndex { get; set; } = 0;

    private PlayLog playLog = new PlayLog();

    private List<ICardEvent> outputActions = new List<ICardEvent>();

    [Header("Sound Effects")]
    public AudioClip ShuffleSE;
    public AudioClip DrawSE;
    public AudioClip TargetArrowSE;
    public AudioClip MoveSE;
    public AudioClip KnockOutSE;
    public AudioClip DamageSE;
    public AudioClip CointTossSE;
    public AudioClip HealSE;
    public AudioClip PoisonSE;
    public AudioClip BurnedSE;
    public AudioClip WinSE;
    public AudioClip LoseSE;
    [SerializeField] AudioClip DecisionSE;
    [SerializeField] AudioClip CancelSE;

    #region Events

    //Cards flipped
    public static Action OnReverseOpponentsCardsChanged;
    public static Action OnCardFlippedChanged;

    public static Action<Player> OnSecurityStackChanged;

    #endregion

    private void Awake()
    {
        selectDNACondition = GetComponent<SelectDNACondition>();
        instance = this;
        commandText.Init();
        StartCoroutine(AwakeCoroutine());
    }

    IEnumerator AwakeCoroutine()
    {
        if (ContinuousController.instance != null)
        {
            if (ContinuousController.instance.isAI)
            {
                IsAI = true;
            }
        }

        if (!IsAI)
        {
            isAuto = false;
        }

        turnStateMachine = gameObject.AddComponent<TurnStateMachine>();

        StartCoroutine(turnStateMachine.Init());

        Debug.Log("Battle Initialization");

        yield return new WaitWhile(() => ContinuousController.instance == null);

        ContinuousController.instance.CanSetRandom = true;

        // Move this later
        ContinuousController.instance.SetRandom((int)DateTime.Now.Ticks);
    }

    public Coroutine OnTargetArrow(Vector3 InitialPosition, Vector3 targetPosition, FieldPermanentCard StartFieldUnitCard, FieldPermanentCard EndFieldUnitCard)
    {
        return null;
    }

    public void OffTargetArrow()
    {
        
    }

    public void BroadcastEvent(ICardEvent action)
    {
        outputActions.Add(action);
    }

    public ICardEvent GetOutputAction(int index)
    {
        if (index >= 0 && index < outputActions.Count)
        {
            return outputActions[index];
        }

        return null;
    }
}
