# pinball-physics
 Documentation about simulating a Pinball game

## Integration

Notations:
- the time being a discreet value is noted $t_i$;
- the acceleration at time $t_i$ is noted $\overrightarrow{a_i}$;
- the speed at time $t_i$ is noted $\overrightarrow{s_i}$;
- the position at time $t_i$ is noted $\overrightarrow{x_i}$.

Given:
- a gravity depending on the current position of the ball $\vec{g}(\overrightarrow{x_N})$;
- speed and position at time $t_N$;
- the time step $\Delta t$ between time $t_N$ and $t_{N+1}$.

We want to compute the position $\overrightarrow{x_{N+1}}$ at time $t_{N+1}$, using the Euler method.

$$ E1: \overrightarrow{a_{N+1}}=\vec{g}(\overrightarrow{x_N}) $$

$$ E2: \overrightarrow{s_{N + 1}} = \overrightarrow{s_N} + \Delta t \times \overrightarrow{a_{N + 1}} $$

$$ E3: \overrightarrow{x_{N+1}} = \overrightarrow{x_N} + \Delta t \times \overrightarrow{s_{N+1}} $$

## Collision detection

Collisions should be detected between $\overrightarrow{x_N}$ and $\overrightarrow{x_{N+1}}$.

If a collision is detected, then the speed $\overrightarrow{s_{N+1}}$ and the position $\overrightarrow{x_{N+1}}$ needs to be changed.

> TODO: collision detection between a disk with a given radius and different kinds of surfaces (flat, curves, other disks, ellipsis, etc.).

### Ball against a point

Given:
- the ball, radius $r_B$, position $\overrightarrow{x}$, speed $\overrightarrow{s}$;
- the point $\vec{p}$.

Algorithm:
1. Normalize the speed: $$ \vec{n}=\frac{\vec{s}}{||\vec{s}||} $$
1. Recenter the point into the ball's frame: $$ \vec{p_B}=\vec{p}-\vec{x} $$
1. If $$ \vec{p_B}\cdot\vec{n} < 0 $$ then the ball is moving away from that point and there couldn't be any collision
1. Optim: If projection of the point to the displacement > radius, no collision
1. Compute the tangent, in order to have the complete ball's frame: $$ \vec{t}=[-\vec{n}_y; \vec{n}_x] $$
1. Compute the projection of the point to the tangent: $$ \vec{c_B}_x=\vec{p_B}\cdot\vec{t} $$.
1. Compute the projection of $\vec{c_B}$ to $\vec{n}$: $$ \vec{c_B}_y=\sqrt{{r_B}^4-{\vec{c_B}_x}^2} $$
1. Compute distance to point along the speed vector: $$ d=||\vec{p_B}-\vec{c_B}|| $$
1. If $$ d \le \Delta t||\vec{s}|| $$ then there will be collision.
1. Compute time before contact: $$ \delta t=\Delta t\times\frac{\Delta t\times||\vec{s}||}{d} $$
1. Compute the normal of contact: $$ \vec{n_c}=-\frac{c_B}{||c_B||} $$

## Continuous collisions

The collision detection algorithms should provide multiple data:
- collision or not;
- time of collision, $t_c$;
- point of collision, $\overrightarrow{p_c}$;
- normal of the surface at collision point, $\overrightarrow{n_c}$ with $||\overrightarrow{n_c}||=1$;
- restitution coefficient of the surface at collision point, $C$.

And we already know:
- speed $\overrightarrow{s_{N+1}}$;
- positions $\overrightarrow{x_N}$ and $\overrightarrow{x_{N+1}}$.

1. Virtually move the ball to the collision point $t_c$, it gives the new position:
   $$ \overrightarrow{x_c}=\overrightarrow{p_c}+r\times \overrightarrow{n_c} $$
2. Compute a tangent on the surface:
   $$ \overrightarrow{t_c}=[-n_c^y;n_c^x] $$
   We may have taken the opposite, it's just an arbitrary choice.
3. "Mirror" the speed $\overrightarrow{s_{N+1}}$ on the surface, keeping the tengential component, but opposing the normal one:
   $$ \overrightarrow{s_{N+1}}=C(\overrightarrow{s_{N+1}}.\overrightarrow{t_c}\times \overrightarrow{t_c} - \overrightarrow{s_{N+1}}.\overrightarrow{n_c}\times \overrightarrow{n_c}) $$
4. Finish the Euler method:
   $$ \overrightarrow{x_{N+1}}=\overrightarrow{x_c}+(t_{N+1}-t_c)\overrightarrow{s_{N+1}} $$

## After the collision

After the collision, the ball can reach another surface, it may be interesting to redo the process again, multiple times until convergence.

This does not treat the friction (yet!).
