using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player 
{
    #region 初期化
    public Player(bool isYouValue)
    {
        isYou = isYouValue;

        fieldCardFrames = new List<FieldCardFrame>();

        for (int i = 0; i < 20; i++)
        {
            FieldCardFrame fieldCardFrame = new FieldCardFrame();
            fieldCardFrame.FrameID = i;
            fieldCardFrame.player = this;
            fieldCardFrames.Add(fieldCardFrame);
        }

        FieldPermanents = new Permanent[fieldCardFrames.Count];
    }
    #endregion

    public bool IsLose { get; set; } = false;

    public void SetLose()
    {
        IsLose = true;
    }

    #region そのプレイヤーの何ターン目か
    public int TurnCount { get; set; } = 0;
    #endregion

    #region 処理領域表示オブジェクト
    public BrainStormObject brainStormObject = new BrainStormObject();
    #endregion

    #region 山札の枚数表示テキスト
    public void SetUpHatchObject(UnityAction OnClickHatchObjectAction)
    {
        
    }
    #endregion

    #region シャッフルアニメーション
    public IEnumerator ShuffleAnimation()
    {
        yield break;
    }
    #endregion

    #region セキュリティ表示
    public SecurityObject securityObject;
    #endregion


    #region カード情報

    #region デッキのカード
    public List<CardSource> LibraryCards = new List<CardSource>();
    #endregion

    #region デジタマデッキのカード
    public List<CardSource> DigitamaLibraryCards = new List<CardSource>();
    #endregion

    #region 手札のカード
    public List<CardSource> HandCards = new List<CardSource>();
    #endregion

    #region 場外のカード
    public List<CardSource> TrashCards = new List<CardSource>();
    #endregion

    #region ロストのカード
    public List<CardSource> LostCards = new List<CardSource>();
    #endregion

    #region ライフのカード
    public List<CardSource> SecurityCards = new List<CardSource>();
    #endregion

    #region 処理領域のカード
    public List<CardSource> ExecutingCards = new List<CardSource>();
    #endregion

    #endregion

    #region このプレイヤーがあなたかどうか
    [Header("このプレイヤーがあなたかどうか")]
    public bool isYou;
    #endregion

    #region 枠
    [Header("パーマネント枠")]
    public List<FieldCardFrame> fieldCardFrames = new List<FieldCardFrame>();
    #endregion

    #region バトルエリアのパーマネント
    public List<Permanent> GetBattleAreaPermanents()
    {
        List<Permanent> GetBattleAreaPermanents = new List<Permanent>();

        for (int i = 0; i < FieldPermanents.Length; i++)
        {
            if (FieldCardFrame.isBattleAreaFrameID(i))
            {
                if (FieldPermanents[i] != null)
                {
                    if (FieldPermanents[i].TopCard != null)
                    {
                        GetBattleAreaPermanents.Add(FieldPermanents[i]);
                    }
                }
            }
        }

        return GetBattleAreaPermanents;
    }
    #endregion

    #region 育成エリアのパーマネント
    public List<Permanent> GetBreedingAreaPermanents()
    {
        List<Permanent> GetBreedingAreaPermanents = new List<Permanent>();

        for (int i = 0; i < FieldPermanents.Length; i++)
        {
            if (!FieldCardFrame.isBattleAreaFrameID(i))
            {
                if (FieldPermanents[i] != null)
                {
                    if (FieldPermanents[i].TopCard != null)
                    {
                        GetBreedingAreaPermanents.Add(FieldPermanents[i]);
                    }
                }
            }
        }

        return GetBreedingAreaPermanents;
    }
    #endregion

    #region 場のパーマネント
    public Permanent[] FieldPermanents = new Permanent[16];

    public List<Permanent> GetFieldPermanents()
    {
        List<Permanent> GetFieldPermanents = new List<Permanent>();

        for (int i = 0; i < FieldPermanents.Length; i++)
        {
            if (FieldPermanents[i] != null)
            {
                if (FieldPermanents[i].TopCard != null)
                {
                    GetFieldPermanents.Add(FieldPermanents[i]);
                }
            }
        }

        return GetFieldPermanents;
    }

    public List<Permanent> GetBattleAreaDigimons()
    {
        List<Permanent> battleAreaPermanents = GetBattleAreaPermanents();
        List<Permanent> GetBattleAreaDigimons = new List<Permanent>();


        for (int i = 0; i < battleAreaPermanents.Count; i++)
        {
            if (battleAreaPermanents[i] != null)
            {
                if (battleAreaPermanents[i].TopCard != null)
                {
                    if (battleAreaPermanents[i].IsDigimon)
                    {
                        GetBattleAreaDigimons.Add(battleAreaPermanents[i]);
                    }
                }
            }
        }

        return GetBattleAreaDigimons;
    }
    #endregion

    #region Player Name
    private string _playerName = "Opponent";
    public string PlayerName {

        get {
            if (isYou || GManager.instance.IsAI)
                return _playerName; 
            else 
                return "Opponent";
        }
        set { _playerName = value; }
    }
    #endregion

    #region PlayerID
    public int PlayerID { get; set; }
    #endregion

    #region Enemy
    public Player Enemy
    {
        get
        {
            if (GManager.instance != null)
            {
                if (GManager.instance.turnStateMachine != null)
                {
                    if (GManager.instance.turnStateMachine.gameContext != null)
                    {
                        if (GManager.instance.turnStateMachine.gameContext.Players.Contains(this))
                        {
                            foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players)
                            {
                                if (player != this)
                                {
                                    return player;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
    #endregion

    #region プレイヤーに掛かっている効果

    #region All effects on the player
    public List<ICardEffect> EffectList(EffectTiming timing)
    {
        List<ICardEffect> PlayerEffects = new List<ICardEffect>();

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in PermanentEffects)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in UntilEndBattleEffects)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in UntilEachTurnEndEffects)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in UntilOwnerTurnEndEffects)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in UntilOwnerActivePhaseEffects)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in UntilSecurityCheckEndEffects)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in UntilOpponentTurnEndEffects)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (Func<EffectTiming, ICardEffect> GetCardEffect in UntilCalculateFixedCostEffect)
        {
            if (GetCardEffect(timing) != null)
            {
                PlayerEffects.Add(GetCardEffect(timing));
            }
        }

        foreach (ICardEffect cardEffect in PlayerEffects)
        {
            if (cardEffect.EffectSourceCard == null)
            {
                foreach (CardSource cardSource in GManager.instance.turnStateMachine.gameContext.ActiveCardList)
                {
                    if (cardSource.Owner == this && !cardSource.IsToken)
                    {
                        cardEffect.SetEffectSourceCard(cardSource);
                        break;
                    }
                }
            }
        }

        return PlayerEffects;
    }
    #endregion

    #region 消えないプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> PermanentEffects = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #region バトル終了時に消えるプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> UntilEndBattleEffects = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #region お互いのターン終了時に消えるプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> UntilEachTurnEndEffects = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #region 自分のターン終了時に消えるプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> UntilOwnerTurnEndEffects = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #region 自分のアクティブフェイズ終了時に消えるプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> UntilOwnerActivePhaseEffects = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #region 相手のターン終了時に消えるプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> UntilOpponentTurnEndEffects = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #region カード使用後に消えるプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> UntilCalculateFixedCostEffect = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #region セキュリティチェック後に消えるプレイヤーに掛かっている効果
    public List<Func<EffectTiming, ICardEffect>> UntilSecurityCheckEndEffects = new List<Func<EffectTiming, ICardEffect>>();
    #endregion

    #endregion

    #region このプレイヤーから見た時のメモリー
    public int MemoryForPlayer
    {
        get
        {
            int memory = GManager.instance.turnStateMachine.gameContext.Memory;

            if (PlayerID == 0)
            {
                memory *= -1;
            }

            return memory;
        }
    }
    #endregion

    #region メモリーを固定の値にする
    public IEnumerator SetFixedMemory(int Memory, ICardEffect cardEffect)
    {
        if (MemoryForPlayer < Memory)
        {
            #region メモリーを+出来ないなら処理終了
            if (cardEffect != null)
            {
                if (!CanAddMemory(cardEffect))
                {
                    yield break;
                }
            }
            #endregion
        }

        if (PlayerID == 0)
        {
            GManager.instance.turnStateMachine.gameContext.Memory = -1 * Memory;
        }

        else
        {
            GManager.instance.turnStateMachine.gameContext.Memory = Memory;
        }

        if (GManager.instance.turnStateMachine.gameContext.Memory >= 10)
        {
            GManager.instance.turnStateMachine.gameContext.Memory = 10;
        }

        else if (GManager.instance.turnStateMachine.gameContext.Memory <= -10)
        {
            GManager.instance.turnStateMachine.gameContext.Memory = -10;
        }

        yield return ContinuousController.instance.StartCoroutine(GManager.instance.memoryObject.SetMemory());
    }
    #endregion

    #region Can you increase memory?
    public bool CanAddMemory(ICardEffect cardEffect)
    {
        if (this.MemoryForPlayer >= 10)
        {
            return false;
        }

        #region Effects that impair memory
        foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players_ForTurnPlayer)
        {
            foreach (Permanent permanent in player.GetFieldPermanents())
            {
                #region Effects of permanents in play
                foreach (ICardEffect cardEffect1 in permanent.EffectList(EffectTiming.None))
                {
                    if (cardEffect1 is ICannotAddMemoryEffect)
                    {
                        if (cardEffect1.CanUse(null))
                        {
                            if (((ICannotAddMemoryEffect)cardEffect1).cannotAddMemory(this, cardEffect))
                            {
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            #region player effect
            foreach (ICardEffect cardEffect1 in player.EffectList(EffectTiming.None))
            {
                if (cardEffect1 is ICannotAddMemoryEffect)
                {
                    if (cardEffect1.CanUse(null))
                    {
                        if (((ICannotAddMemoryEffect)cardEffect1).cannotAddMemory(this, cardEffect))
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        return true;
    }
    #endregion

    #region メモリーを+する
    public IEnumerator AddMemory(int plusMemory, ICardEffect cardEffect)
    {
        if (plusMemory == 0)
        {
            yield break;
        }

        if (plusMemory >= 1)
        {
            #region メモリーを+出来ないなら処理終了
            if (cardEffect != null)
            {
                if (!CanAddMemory(cardEffect))
                {
                    yield break;
                }
            }
            #endregion
        }

        if (PlayerID == 0)
        {
            GManager.instance.turnStateMachine.gameContext.Memory -= plusMemory;
        }

        else
        {
            GManager.instance.turnStateMachine.gameContext.Memory += plusMemory;
        }

        if (GManager.instance.turnStateMachine.gameContext.Memory >= 10)
        {
            GManager.instance.turnStateMachine.gameContext.Memory = 10;
        }

        else if (GManager.instance.turnStateMachine.gameContext.Memory <= -10)
        {
            GManager.instance.turnStateMachine.gameContext.Memory = -10;
        }

        yield return ContinuousController.instance.StartCoroutine(GManager.instance.memoryObject.SetMemory());
    }
    #endregion

    #region 払えるメモリーコストの上限
    public int MaxMemoryCost
    {
        get
        {
            int MaxMemoryCost = 0;

            if (PlayerID == 0)
            {
                MaxMemoryCost = Mathf.Abs(10 - GManager.instance.turnStateMachine.gameContext.Memory);
            }

            else
            {
                MaxMemoryCost = Mathf.Abs(-10 - GManager.instance.turnStateMachine.gameContext.Memory);
            }

            return MaxMemoryCost;
        }
    }
    #endregion

    #region コスト支払い後のメモリー予測値
    public int ExpectedMemory(int memoryCost)
    {
        int ExpectedMemory = GManager.instance.turnStateMachine.gameContext.Memory;

        if (PlayerID == 0)
        {
            ExpectedMemory += memoryCost;
        }

        else
        {
            ExpectedMemory -= memoryCost;
        }

        return ExpectedMemory;
    }
    #endregion

    #region デジタマを孵化できるか
    public bool CanHatch => DigitamaLibraryCards.Count >= 1 && GetBreedingAreaPermanents().Count == 0;
    #endregion

    #region デジモンを移動できるか
    public bool CanMove => GetBreedingAreaPermanents().Count(permanent => permanent.CanMove) >= 1 && fieldCardFrames.Count((frame) => frame.IsEmptyFrame()) >= 1;
    #endregion

    #region このターンにデジモンを進化させた回数
    public int DigivolveCount_ThisTurn { get; set; } = 0;
    #endregion

    #region 吸収進化でタップできるデジモンの条件(効果可否判定に使用)
    public bool CanTapWhenAbsorbEvolution_CheckAvailability(Permanent permanent, ICardEffect cardEffect)
    {
        if (permanent != null)
        {
            if (permanent.TopCard != null)
            {
                if (!permanent.IsSuspended && permanent.CanSuspend)
                {
                    if (permanent.TopCard.Owner.GetBattleAreaDigimons().Contains(permanent))
                    {
                        #region 吸収進化でタップできるデジモンの条件を変更させる効果
                        foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players_ForTurnPlayer)
                        {
                            foreach (Permanent permanent1 in player.GetFieldPermanents())
                            {
                                #region 場のパーマネントの効果
                                foreach (ICardEffect cardEffect1 in permanent1.EffectList(EffectTiming.None))
                                {
                                    if (cardEffect1 is ICanSuspendByDigisorptionEffect)
                                    {
                                        if (cardEffect1.CanUse(null))
                                        {
                                            if (((ICanSuspendByDigisorptionEffect)cardEffect1).isCheckAvailability())
                                            {
                                                if (((ICanSuspendByDigisorptionEffect)cardEffect1).canSuspendDigisorption(permanent, cardEffect))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            #region プレイヤーの効果
                            foreach (ICardEffect cardEffect1 in player.EffectList(EffectTiming.None))
                            {
                                if (cardEffect1 is ICanSuspendByDigisorptionEffect)
                                {
                                    if (cardEffect1.CanUse(null))
                                    {
                                        if (((ICanSuspendByDigisorptionEffect)cardEffect1).isCheckAvailability())
                                        {
                                            if (((ICanSuspendByDigisorptionEffect)cardEffect1).canSuspendDigisorption(permanent, cardEffect))
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion

                        if (permanent.TopCard.Owner == this)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    #endregion

    #region 吸収進化でタップできるデジモンの条件
    public bool CanTapWhenAbsorbEvolution(Permanent permanent, ICardEffect cardEffect)
    {
        if (permanent != null)
        {
            if (permanent.TopCard != null)
            {
                if (!permanent.IsSuspended && permanent.CanSuspend)
                {
                    if (permanent.TopCard.Owner.GetBattleAreaDigimons().Contains(permanent))
                    {
                        #region 吸収進化でタップできるデジモンの条件を変更させる効果
                        foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players_ForTurnPlayer)
                        {
                            foreach (Permanent permanent1 in player.GetFieldPermanents())
                            {
                                #region 場のパーマネントの効果
                                foreach (ICardEffect cardEffect1 in permanent1.EffectList(EffectTiming.None))
                                {
                                    if (cardEffect1 is ICanSuspendByDigisorptionEffect)
                                    {
                                        if (cardEffect1.CanUse(null))
                                        {
                                            if (!((ICanSuspendByDigisorptionEffect)cardEffect1).isCheckAvailability())
                                            {
                                                if (((ICanSuspendByDigisorptionEffect)cardEffect1).canSuspendDigisorption(permanent, cardEffect))
                                                {
                                                    return true;
                                                }

                                                else
                                                {
                                                    return false;
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }

                            #region プレイヤーの効果
                            foreach (ICardEffect cardEffect1 in player.EffectList(EffectTiming.None))
                            {
                                if (cardEffect1 is ICanSuspendByDigisorptionEffect)
                                {
                                    if (cardEffect1.CanUse(null))
                                    {
                                        if (!((ICanSuspendByDigisorptionEffect)cardEffect1).isCheckAvailability())
                                        {
                                            if (((ICanSuspendByDigisorptionEffect)cardEffect1).canSuspendDigisorption(permanent, cardEffect))
                                            {
                                                return true;
                                            }

                                            else
                                            {
                                                return false;
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion

                        if (permanent.TopCard.Owner == this)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    #endregion

    #region コストを減らせるか
    public bool CanReduceCost(List<Permanent> targetPermanents, CardSource cardSource)
    {
        #region コストを減らせなくさせる効果
        foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players_ForTurnPlayer)
        {
            foreach (Permanent permanent in player.GetFieldPermanents())
            {
                #region 場のパーマネントの効果
                foreach (ICardEffect cardEffect in permanent.EffectList(EffectTiming.None))
                {
                    if (cardEffect is ICannotReduceCostEffect)
                    {
                        if (cardEffect.CanUse(null))
                        {
                            if (((ICannotReduceCostEffect)cardEffect).CannotReduceCost(this, targetPermanents, cardSource))
                            {
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            #region プレイヤーの効果
            foreach (ICardEffect cardEffect in player.EffectList(EffectTiming.None))
            {
                if (cardEffect is ICannotReduceCostEffect)
                {
                    if (cardEffect.CanUse(null))
                    {
                        if (((ICannotReduceCostEffect)cardEffect).CannotReduceCost(this, targetPermanents, cardSource))
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        return true;
    }
    #endregion

    #region DP消滅効果の上限
    public int MaxDP_DeleteEffect(int maxDP, ICardEffect cardEffect)
    {
        int _maxDP = maxDP;

        #region DP消滅効果の上限を変更する効果

        foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players_ForTurnPlayer)
        {
            foreach (Permanent permanent in player.GetFieldPermanents())
            {
                #region 場のパーマネントの効果
                foreach (ICardEffect cardEffect1 in permanent.EffectList(EffectTiming.None))
                {
                    if (cardEffect1 is IChangeDPDeleteEffectMaxDPEffect)
                    {
                        if (cardEffect1.CanUse(null))
                        {
                            _maxDP = ((IChangeDPDeleteEffectMaxDPEffect)cardEffect1).GetMaxDP(_maxDP, cardEffect);
                        }
                    }
                }
                #endregion
            }

            #region プレイヤーの効果
            foreach (ICardEffect cardEffect1 in player.EffectList(EffectTiming.None))
            {
                if (cardEffect1 is IChangeDPDeleteEffectMaxDPEffect)
                {
                    if (cardEffect1.CanUse(null))
                    {
                        _maxDP = ((IChangeDPDeleteEffectMaxDPEffect)cardEffect1).GetMaxDP(_maxDP, cardEffect);
                    }
                }
            }
            #endregion
        }

        #endregion

        return _maxDP;
    }
    #endregion

    #region 進化条件を無視できるか
    public bool CanIgnoreDigivolutionRequirement(Permanent targetPermanent, CardSource cardSource)
    {
        #region 進化条件を無視できなくさせる効果
        foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players_ForTurnPlayer)
        {
            foreach (Permanent permanent in player.GetFieldPermanents())
            {
                #region 場のパーマネントの効果
                foreach (ICardEffect cardEffect1 in permanent.EffectList(EffectTiming.None))
                {
                    if (cardEffect1 is ICannotIgnoreDigivolutionConditionEffect)
                    {
                        if (cardEffect1.CanUse(null))
                        {
                            if (((ICannotIgnoreDigivolutionConditionEffect)cardEffect1).cannotIgnoreDigivolutionCondition(this, targetPermanent, cardSource))
                            {
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            #region Player effect
            foreach (ICardEffect cardEffect1 in player.EffectList(EffectTiming.None))
            {
                if (cardEffect1 is ICannotIgnoreDigivolutionConditionEffect)
                {
                    if (cardEffect1.CanUse(null))
                    {
                        if (((ICannotIgnoreDigivolutionConditionEffect)cardEffect1).cannotIgnoreDigivolutionCondition(this, targetPermanent, cardSource))
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        return true;
    }
    #endregion

    #region セキュリティを増やせるか
    public bool CanAddSecurity(ICardEffect cardEffect)
    {
        if (GManager.instance.turnStateMachine.gameContext.IsSecurityLooking)
        {
            return false;
        }

        #region セキュリティを増やせない効果
        foreach (Player player in GManager.instance.turnStateMachine.gameContext.Players_ForTurnPlayer)
        {
            foreach (Permanent permanent in player.GetFieldPermanents())
            {
                #region 場のパーマネントの効果
                foreach (ICardEffect cardEffect1 in permanent.EffectList(EffectTiming.None))
                {
                    if (cardEffect1 is ICannotAddSecurityEffect)
                    {
                        if (cardEffect1.CanUse(null))
                        {
                            if (((ICannotAddSecurityEffect)cardEffect1).cannotAddSecurity(this, cardEffect))
                            {
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            #region プレイヤーの効果
            foreach (ICardEffect cardEffect1 in player.EffectList(EffectTiming.None))
            {
                if (cardEffect1 is ICannotAddSecurityEffect)
                {
                    if (cardEffect1.CanUse(null))
                    {
                        if (((ICannotAddSecurityEffect)cardEffect1).cannotAddSecurity(this, cardEffect))
                        {
                            return false;
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        return true;
    }
    #endregion

    #region セキュリティを減らせるか
    public bool CanReduceSecurity()
    {
        if (GManager.instance.turnStateMachine.gameContext.IsSecurityLooking)
        {
            return false;
        }

        return true;
    }
    #endregion
}

[Serializable]
public class FieldCardFrame
{
    [Header("枠ID")]
    public int FrameID;

    [Header("プレイヤー")]
    public Player player;

    public static bool isBattleAreaFrameID(int FrameID)
    {
        return 0 <= FrameID && FrameID <= GManager.instance.You.fieldCardFrames.Count - 2;
    }
    public bool IsBattleAreaFrame()
    {
        return isBattleAreaFrameID(this.FrameID);
    }

    public bool isBreedingAreaFrame()
    {
        return !IsBattleAreaFrame();
    }

    Permanent framePermanent = null;

    public void SetFramePermanent(Permanent permanent)
    {
        framePermanent = permanent;
    }

    public Permanent GetFramePermanent()
    {
        /*
        foreach (Permanent permanent in player.GetFieldPermanents())
        {
            if (permanent.PermanentFrame == this)
            {
                return permanent;
            }
        }
        */

        if (framePermanent != null)
        {
            return framePermanent;
        }

        return null;
    }

    public bool IsEmptyFrame()
    {
        return GetFramePermanent() == null;
    }
}
