# Pinball physics

Documentation about simulating a Pinball game.

- [1. Conventions and notations](#1-conventions-and-notations)
- [2. Integration](#2-integration)
- [3. Interpenetration](#3-interpenetration)
- [4. Continuous collision detection](#4-continuous-collision-detection)
  - [4.1. When collider is a point](#41-when-collider-is-a-point)
- [5. Continuous collision reaction](#5-continuous-collision-reaction)
- [6. After the collision](#6-after-the-collision)

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

## 3. Interpenetration

> TODO: It may be interesting to detect interpenetrations before the continuous collision detection.

## 4. Continuous collision detection

Collisions should be detected between $\vec{x}_i$ and $\vec{x}_{i+1}$.

We assume that there is no interpenetration at time $t_i$, at most the ball is touching a collider, which may be resolve as a collision with $\delta t=0$.

If a collision is detected, then the speed $\vec{s}_{i+1}$ and the position $\vec{x}_{i+1}$ needs to be changed, and the contact point $\vec{c}$, the time to collision $\delta t$ and the normal $\vec{N}$ of the collider at $\vec{c}$ should be returned in order to allow the collision reaction algorithm to be executed.

### 4.1. When collider is a point

Given:
- the point $\vec{p}\in\mathbb{R}^2$.

One create a frame local to the moving ball, centered at $\vec{x}$ with base 
$$\mathscr{B}=\left\{\vec{t},\vec{n}\right\},\vec{n}=\frac{\vec{s}}{||\vec{s}||},\vec{t}=\left[-\vec{n}_y;\vec{n}_x\right]$$

Algorithm:
1. Let $\vec{xp}=\vec{p}-\vec{x}$.
2. If $\vec{xp}\cdot\vec{n}\le0$ then the ball is globally moving away from that point and there couldn't be any collision.
3. Compute the projection of the point to the tangent:
   $$\vec{xc}_x=\vec{xp}\cdot\vec{t}$$
   If $||\vec{xc}_x||\notin\left]-r,r\right[$ then the ball will not touch the point.
4. Compute the projection of $\vec{xc}$ to $\vec{n}$ (we know that $||\vec{xc}||=r$, thus invoking Pythagore):
   $$ \vec{xc}_y=\sqrt{r^2-\vec{xc}_x^2} $$
5. Compute distance to point along the speed vector:
   $$ d=||\vec{xp}-\vec{xc}|| $$
6. Compute the time before contact:
   $$ \delta t=\frac{d}{||\vec{s}||} $$
   If $\delta t\ge\Delta t$ there will be no collision for that iteration. The particular case $\delta t=\Delta t$ will be handled in the next iteration.
7. Compute the normal of contact:
   $$ \vec{N}=-\frac{\vec{xc}}{||\vec{xc}||} $$

## 5. Continuous collision reaction

The collision detection algorithms should provide multiple data:
- collision or not;
- time of collision, $\delta t$;
- point of collision, $\vec{c}$;
- normal of the surface at collision point, $\vec{N}$ with $||\vec{N}||=1$;
- restitution coefficient of the surface at collision point, $C$.

And we already know:
- speed $\vec{s}_{N+1}$;
- positions $\vec{x}_N$ and $\vec{x}_{N+1}$.

1. Virtually move the ball to the collision point $t_i+\delta t$, it gives the new position:
   $$ \vec{x_c}=\vec{c}+r\times \vec{N} $$
2. Compute a tangent on the surface:
   $$ \vec{T}=[-\vec{N}_y;\vec{N}_x] $$
3. "Mirror" the speed $\vec{s}_{i+1}$ on the surface, keeping the tengential component, but reflecting the normal one:
   $$ \vec{s}_{i+1}=(\vec{s}_{i+1}\cdot\vec{T})\vec{T} - C(\vec{s}_{i+1}\cdot\vec{N})\vec{N} $$
4. Finish the Euler method:
   $$ \vec{x}_{i+1}=\vec{x_c}+(\Delta t-\delta t)\cdot\vec{s}_{i+1} $$

## 6. After the collision

After the collision, the ball can reach another surface before $t_{i+1}$, it may be interesting to redo the process again, multiple times until convergence.

This does not treat the friction (yet!).
