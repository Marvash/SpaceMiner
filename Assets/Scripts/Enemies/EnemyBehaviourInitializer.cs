public class EnemyBehaviourInitializer {
    private PlayershipManagerSO _playershipManagerSO;
    public EnemyBehaviourInitializer(PlayershipManagerSO playershipManagerSO) {
        _playershipManagerSO = playershipManagerSO;
    }
    public void InitSimpleEnemy(ISimpleEnemy simpleEnemy) {
        ISimpleEnemy enemy = simpleEnemy.GetComponent<ISimpleEnemy>();
        enemy.SetTarget(_playershipManagerSO.Player);
    }
}