using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Task
{
    Idle,
    EatPlant,
    ChargePlant,
    ShootPlayer,
    MeleePlayer,
    ChargePlayer
}

public class Enemy : Entity
{
    //[SerializeField] ItemHolder itemHolder;
    //[SerializeField] float viewDistance;
    //[SerializeField] Vector2 fireDuration;
    //[SerializeField] Vector2 restDuration;
    //[SerializeField] Vector2 delayShootOnSee;
    //[SerializeField] Vector2 aimVariability;
    //[SerializeField] bool periodicShooting;

    [SerializeField] float targetDistanceFromPlayer;
    [SerializeField] float eatRate;
    [SerializeField] int eatDamage;

    public WorldPlant Plant { get; private set; }
    Character player;

    System.Action CurrentAction;
    Vector3 lastSeenPosition;
    Task task;

    Vector3 moveDir;

    private void Start()
    {
        SetTask(Task.Idle);
    }

    public void Setup(Character player)
    {
        this.player = player;
    }

    public void AssignPlant(WorldPlant plant)
    {
        if (dead) return;

        this.Plant = plant;
        Plant.Destroyed += PlantDestroyed;
        SetTask(Task.ChargePlant, false);
    }

    void PlantDestroyed(WorldObject wo)
    {
        Plant = null;
    }

    void SetTask(Task t, bool dontIfAlreadyIs = true)
    {
        if (dontIfAlreadyIs == false || t != CurrentTask)
            CurrentTask = t;
    }

    public virtual Task CurrentTask
    {
        get
        {
            return task;
        }
        set
        {
            CurrentAction = null;
            StopAllCoroutines();

            print($"{value.ToString()}");
            switch (value)
            {
                case Task.Idle: StopMoving(); break;
                case Task.ChargePlant:

                    moveDir = new Vector3(Plant.X, Plant.Y) - transform.position;
                    CurrentAction += ChargePlant;

                    break;
                case Task.EatPlant:

                    StopMoving();
                    StartCoroutine(Eat());

                    break;
                case Task.ChargePlayer:

                    CurrentAction += ChargePlayer;

                    break;
                case Task.ShootPlayer:

                    CurrentAction += Shoot;

                    break;
                case Task.MeleePlayer:

                    StopMoving();
                    StartCoroutine(MeleePlayer());

                    break;
            }

            task = value;
        }
    }

    public virtual void FixedUpdate()
    {
        DoLogic();
        CurrentAction?.Invoke();
    }

    float DistanceToPlant()
    {
        return Vector2.Distance(transform.position, new Vector2(Plant.X, Plant.Y));
    }

    float DistanceToPlayer()
    {
        return Vector2.Distance(player.transform.position, transform.position);
    }

    public virtual void DoLogic()
    {
        float distToPlayer = DistanceToPlayer();

        if (Plant == null)
        {
            if (distToPlayer > .75f && CurrentTask != Task.MeleePlayer)
            {
                SetTask(Task.ChargePlayer);
                return;
            }
            else if (distToPlayer < 1)
                SetTask(Task.MeleePlayer);
            else
                SetTask(Task.ChargePlayer);
        }
        else
        {
            float distToPlant = DistanceToPlant();

            //if (distToPlant / 3 > distToPlayer)
            //{
            //    if (distToPlayer > .5f && CurrentTask != Task.MeleePlayer)
            //    {
            //        SetTask(Task.ChargePlayer);
            //    }
            //    else if (distToPlayer < 1)
            //    {
            //        SetTask(Task.MeleePlayer);
            //    }
            //    else
            //        SetTask(Task.ChargePlayer);

            //    return;
            //}

            if (distToPlant > 1)
                SetTask(Task.ChargePlant);
            else
                SetTask(Task.EatPlant);
        }
    }

    public override void Die()
    {
        //if (ih.pickedUpItem)
        //{
        //    ih.DropItem();
        //}

        // Stop bursts from firing
        //itemUser?.StopAllCoroutines();

        base.Die();
    }

    public override void Damage(int val, Vector2 knockBack)
    {
        base.Damage(val, knockBack);

        //if (ih.Item && ih.Item.healAmount > 0)
        //{
        //    ih.TryUseItem();
        //}
    }

    public bool CanSeePlayer()
    {
        //if (Vector2.Distance(transform.position, player.transform.position) > viewDistance) { return false; }
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, viewDistance, ~(((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Pickup")) | (1 << LayerMask.NameToLayer("PlayerBullet") | (1 << LayerMask.NameToLayer("EnemyBullet"))))));
        //return (hit.collider && hit.collider.CompareTag("Player"));
        return false;
    }

    #region ShootPlayer

    bool canShoot;

    void Shoot()
    {
        //if (canShoot || icont.HasDefaultItem == false)
        //{
        //    icont.TryUseItem();
        //}
    }

    //IEnumerator ShootDelay()
    //{
    //    canShoot = false;
    //    yield return new WaitForSeconds(Random.Range(delayShootOnSee.x, delayShootOnSee.y));

    //    if (periodicShooting)
    //        StartCoroutine(ShootPeriod());

    //    canShoot = true;
    //}

    //IEnumerator ShootPeriod()
    //{
    //    canShoot = true;
    //    yield return new WaitForSeconds(Random.Range(fireDuration.x, fireDuration.y));

    //    canShoot = false;
    //    yield return new WaitForSeconds(Random.Range(restDuration.x, restDuration.y));
    //    StartCoroutine(ShootPeriod());
    //}

    #endregion

    #region ChargePlant

    public virtual void ChargePlant()
    {
        MoveTowardsPos(new Vector3(Plant.X, Plant.Y));
    }

    #endregion

    #region ChargePlayer

    void ChargePlayer()
    {
        MoveTowardsPos(player.transform.position);
    }

    #endregion

    #region MeleePlayer

    IEnumerator MeleePlayer()
    {
        player.Damage(eatDamage, player.transform.position - transform.position);
        yield return new WaitForSeconds(eatRate);
        StartCoroutine(MeleePlayer());
    }

    #endregion

    #region Eat

    IEnumerator Eat()
    {
        Plant.TakeDamage(eatDamage);
        yield return new WaitForSeconds(eatRate);
        StartCoroutine(Eat());
    }

    #endregion
}







//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public enum Task
//{
//    Idle,
//    EatPlant,
//    ChargePlant,
//    ShootPlayer,
//    ChargePlayer
//}

//[System.Serializable]
//public class State
//{
//    [SerializeField] Task task;
//    [SerializeField] bool requirePlant;
//    //[SerializeField] float minimumDistanceToPlayer closerToPlant;
//    [SerializeField] bool closerToPlayer;
//    [SerializeField] float atLeastThisCloseToPlant;
//    [SerializeField] bool atLeastThisCloseToPlantRequirement;

//    public Task Task { get { return task; } }
//    public bool RequirePlant { get { return requirePlant; } }
//    public bool CloserToPlayer { get { return closerToPlayer; } }
//    //public bool RequirePickedUpItem { get { return requirePickedUpItem; } }
//    public float MaxDistanceFromPlant { get { return atLeastThisCloseToPlant; } }
//}

//public class Enemy : Entity
//{
//    //[SerializeField] ItemHolder itemHolder;
//    //[SerializeField] float viewDistance;
//    //[SerializeField] Vector2 fireDuration;
//    //[SerializeField] Vector2 restDuration;
//    //[SerializeField] Vector2 delayShootOnSee;
//    //[SerializeField] Vector2 aimVariability;
//    //[SerializeField] bool periodicShooting;

//    [SerializeField] float targetDistanceFromPlayer;
//    [SerializeField] List<State> states;
//    [SerializeField] float eatDamage;
//    [SerializeField] float eatRate;

//    public WorldPlant Plant { get; private set; }
//    Character player;

//    System.Action CurrentAction;
//    Vector3 lastSeenPosition;
//    Task task;

//    float distanceFromTarget;
//    Vector3 moveDir;

//    private void Start()
//    {
//        SetTask(Task.Idle);
//    }

//    public void Setup(Character player)
//    {
//        this.player = player;
//    }

//    public void AssignPlant(WorldPlant plant)
//    {
//        this.Plant = plant;
//        Plant.Destroyed += PlantDestroyed;

//        SetTask(Task.ChargePlant, false);
//    }

//    void PlantDestroyed(WorldObject wo)
//    {
//        Plant = null;
//    }

//    void SetTask(Task t, bool dontIfAlreadyIs = true)
//    {
//        if (dontIfAlreadyIs || t != CurrentTask)
//            CurrentTask = t;
//    }

//    public virtual Task CurrentTask
//    {
//        get
//        {
//            return task;
//        }
//        set
//        {
//            StopAllCoroutines();

//            if (value == Task.ChargePlant)
//            {
//                moveDir = new Vector3(Plant.X, Plant.Y) - transform.position;
//                CurrentAction = ChargePlant;
//            }
//            else if (value == Task.EatPlant)
//            {
//                StartCoroutine(Eat());
//            }
//            else if (value == Task.ShootPlayer)
//            {
//                CurrentAction = Shoot;
//                //StartCoroutine(ShootDelay());
//            }

//            task = value;
//        }
//    }

//    Task GetBestTask()
//    {
//        for (int i = 0; i < states.Count; i++)
//        {
//            State s = states[i];

//            if (s.RequirePlant)
//            {
//                if (Plant == null) continue;
//                else if (DistanceToPlant() > s.MaxDistanceFromPlant) continue;
//            }

//            if (s.CloserToPlayer)
//            {
//                if (Plant == null || DistanceToPlant() < DistanceToPlayer()) continue;
//            }


//            return s.Task;
//        }

//        return Task.Idle;
//    }

//    public virtual void FixedUpdate()
//    {
//        DoLogic();
//        CurrentAction?.Invoke();
//    }

//    float DistanceToPlant()
//    {
//        return Vector2.Distance(transform.position, new Vector2(Plant.X, Plant.Y));
//    }

//    float DistanceToPlayer()
//    {
//        return Vector2.Distance(transform.position, player.transform.position);
//    }

//    public virtual void DoLogic()
//    {
//        SetTask(GetBestTask(), true);

//        //if (Plant == null) return;

//        //distanceFromTarget = Vector2.Distance(transform.position, new Vector2(Plant.X, Plant.Y));

//        //// Moving toward plant.
//        //if (CurrentTask == State.tryEating)
//        //{
//        //    // Close enough to eat
//        //    if (distanceFromTarget <= 1)
//        //        // Start chewing
//        //        if (CurrentTask != State.eating)
//        //            CurrentTask = State.eating;
//        //}
//        //// If I am not eating and I am closer to a plant move toward it.
//        //else if (Vector2.Distance(transform.position, player.transform.position) > distanceFromTarget)
//        //{
//        //    if (CurrentTask != State.tryEating)
//        //        CurrentTask = State.tryEating;
//        //}
//        //else
//        //{
//        //    if (CurrentTask != State.shooting)
//        //        CurrentTask = State.shooting;
//        //}
//    }

//    public override void Die()
//    {
//        //if (ih.pickedUpItem)
//        //{
//        //    ih.DropItem();
//        //}

//        // Stop bursts from firing
//        //itemUser?.StopAllCoroutines();

//        base.Die();
//    }

//    public override void Damage(int val, Vector2 knockBack)
//    {
//        base.Damage(val, knockBack);

//        //if (ih.Item && ih.Item.healAmount > 0)
//        //{
//        //    ih.TryUseItem();
//        //}
//    }

//    public bool CanSeePlayer()
//    {
//        //if (Vector2.Distance(transform.position, player.transform.position) > viewDistance) { return false; }
//        //RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, viewDistance, ~(((1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Pickup")) | (1 << LayerMask.NameToLayer("PlayerBullet") | (1 << LayerMask.NameToLayer("EnemyBullet"))))));
//        //return (hit.collider && hit.collider.CompareTag("Player"));
//        return false;
//    }

//    //float AngleToPlayer()
//    //{
//    //    return 0;
//    //    return Extensions.AngleFromPosition(ih.transform.position, player.transform.position);
//    //}

//    #region ShootPlayer

//    bool canShoot;

//    void Shoot()
//    {
//        //if (canShoot || icont.HasDefaultItem == false)
//        //{
//        //    icont.TryUseItem();
//        //}
//    }

//    //IEnumerator ShootDelay()
//    //{
//    //    canShoot = false;
//    //    yield return new WaitForSeconds(Random.Range(delayShootOnSee.x, delayShootOnSee.y));

//    //    if (periodicShooting)
//    //        StartCoroutine(ShootPeriod());

//    //    canShoot = true;
//    //}

//    //IEnumerator ShootPeriod()
//    //{
//    //    canShoot = true;
//    //    yield return new WaitForSeconds(Random.Range(fireDuration.x, fireDuration.y));

//    //    canShoot = false;
//    //    yield return new WaitForSeconds(Random.Range(restDuration.x, restDuration.y));
//    //    StartCoroutine(ShootPeriod());
//    //}

//    #endregion

//    #region TryEat

//    public virtual void ChargePlant()
//    {
//        if (distanceFromTarget > 1)
//        {
//            MoveTowardsPos(moveDir + transform.position);
//        }
//    }

//    #endregion

//    #region Eating

//    IEnumerator Eat()
//    {
//        Plant.TakeDamage(eatDamage);
//        yield return new WaitForSeconds(eatRate);
//        StartCoroutine(Eat());
//    }

//    #endregion
//}
