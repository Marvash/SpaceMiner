public class EnemyBehaviourInitializer {
    private GameManagerSO gameManager;
    public EnemyBehaviourInitializer(GameManagerSO gameManager) {
        this.gameManager = gameManager;
    }
    public void InitSimpleEnemy(ISimpleEnemy simpleEnemy) {
        ISimpleEnemy enemy = simpleEnemy.GetComponent<ISimpleEnemy>();
        enemy.SetTarget(gameManager.Player);
    }
}