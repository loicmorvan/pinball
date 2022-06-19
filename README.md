# Pinball physics

Documentation about simulating a Pinball game.

- [1. Conventions and notations](#1-conventions-and-notations)
- [2. Integration](#2-integration)
- [3. Continuous collision detection](#3-continuous-collision-detection)
  - [3.1. Ball against a point](#31-ball-against-a-point)
- [4. Continuous collision reaction](#4-continuous-collision-reaction)
- [5. After the collision](#5-after-the-collision)

## 1. Conventions and notations

This is a 2D simulation with no elevation. We assume the simulation will run in real-time at arround 60Hz, but it should work with other framerates, even if it is not perfectly constant.

Notations:
- the algorithm is iterative, each step is noted $i\in \mathbb{N}$ and consist of using the state $i$ in order to compute the state $i+1$
- the time at iteration $i$ is noted $t_i\in \mathbb{R}^+$
- the timestep between two consecutive iterations $i$ and $i+1$ is noted ${\Delta t}_i=t_{i+1}-t_i$
- the ball is a disk (!) of radius $r\in \mathbb{R}^{+*}$
- the 2D acceleration of the ball at time $t_i$ is noted $\vec{a}_i\in \mathbb{R}^2$
- the 2D speed of the ball at time $t_i$ is noted $\vec{s}_i\in \mathbb{R}^2$
- the 2D position of the ball at time $t_i$ is noted $\vec{x}_i\in \mathbb{R}^2$
- the gravity at any position is noted $\vec{g}:\mathbb{R}^2\rightarrow\mathbb{R}^2$ (note that an elevation can be added at that point in order to simulate ramps)

When we are focusing on a specific iteration $i$, we may ommit $\square_i$ as it is implied.

## 2. Integration

Initial values (especially $\vec{x}_0$) depends on the board, but can be set like these:
$$ \vec{a}_0=\vec{0}, \vec{s}_0=\vec{0}, \vec{x}_0=\vec{0} $$

At iteration $i$ we compute the acceleration, speed and position for the next state $i+1$ using the Euler method:

$$ \vec{a}_{i+1}=\vec{g}(\vec{x}_i) $$
$$ \vec{s}_{i+1} = \vec{s}_i + {\Delta t}_i \times \vec{a}_{i+1} $$
$$ \vec{x}_{i+1} = \vec{x}_i + {\Delta t}_i \times \vec{s}_{i+1} $$

## 3. Continuous collision detection

Collisions should be detected between $\vec{x}_i$ and $\vec{x}_{i+1}$.

We assume that there is no interpenetration at time $t_i$, at most the ball is touching a collider, which may be resolve as a collision with $\delta t=0$.

If a collision is detected, then the speed $\vec{s}_{i+1}$ and the position $\vec{x}_{i+1}$ needs to be changed.

### 3.1. Ball against a point

Given:
- the point $\vec{p}\in \mathbb{R}^2$.

One want to consider a "frame of the moving ball", centered at $\vec{x}$ with base $B=\{\vec{t},\vec{n}\}$ with $\vec{n}=\frac{\vec{s}}{||\vec{s}||}$ and $\vec{t}=[-\vec{n}_y; \vec{n}_x]$.

Algorithm:
1. Let $\vec{xp}=\vec{p}-\vec{x}$.
2. If $\vec{xp}\cdot\vec{n} < 0$ then the ball is moving away from that point and there couldn't be any collision
3. Compute the projection of the point to the tangent:
   $$ \vec{xc}_x=\vec{xp}\cdot\vec{t} $$
4. Compute the projection of $\vec{c_B}$ to $\vec{n}$:
   $$ \vec{xc}_y=\sqrt{{r}^4-{\vec{xc}_x}^2} $$
5. Compute distance to point along the speed vector:
   $$ d=||\vec{xp}-\vec{xc}||=||\vec{p}-\vec{c}|| $$
6. If 
   $$ d \le \Delta t||\vec{s}|| $$
   then there will be collision.
7.  Compute time before contact:
    $$ \delta t=\Delta t\times\frac{\Delta t\times||\vec{s}||}{d} $$
8.  Compute the normal of contact:
    $$ \vec{n_c}=-\frac{c_B}{||c_B||} $$

## 4. Continuous collision reaction

The collision detection algorithms should provide multiple data:
- collision or not;
- time of collision, $\delta t$;
- point of collision, $\vec{p_c}$;
- normal of the surface at collision point, $\vec{n_c}$ with $||\vec{n_c}||=1$;
- restitution coefficient of the surface at collision point, $C$.

And we already know:
- speed $\vec{s}_{N+1}$;
- positions $\vec{x}_N$ and $\vec{x}_{N+1}$.

1. Virtually move the ball to the collision point $t_c$, it gives the new position:
   $$ \vec{x_c}=\vec{p_c}+r\times \vec{n_c} $$
2. Compute a tangent on the surface:
   $$ \vec{t_c}=[-n_c^y;n_c^x] $$
   We may have taken the opposite, it's just an arbitrary choice.
3. "Mirror" the speed $\vec{s}_{N+1}$ on the surface, keeping the tengential component, but opposing the normal one:
   $$ \vec{s}_{N+1}=C(\vec{s}_{N+1}.\vec{t_c}\times \vec{t_c} - \vec{s}_{N+1}.\vec{n_c}\times \vec{n_c}) $$
4. Finish the Euler method:
   $$ \vec{x}_{N+1}=\vec{x_c}+(t_{N+1}-t_c)\vec{s}_{N+1} $$

## 5. After the collision

After the collision, the ball can reach another surface, it may be interesting to redo the process again, multiple times until convergence.

This does not treat the friction (yet!).
