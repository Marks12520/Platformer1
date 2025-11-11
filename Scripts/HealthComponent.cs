using Godot;

public partial class HealthComponent : Node
{
    [Export] public int maxHealth;
    private int health;
    
    public void Damage(int damage)
    {
        if (GetOwner().Name == "Player")
        {
            Global.Instance.Health -= damage;
            if (Global.Instance.Health <= 0)
            {
                GetParent().Call("HandleDeath");
            }
        }
        else
        {
            health -= damage;
            if (health <= 0)
            {
                GetParent().Call("HandleDeath");
            }
        }
    }
}
