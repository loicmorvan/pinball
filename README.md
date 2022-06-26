# Pinball physics

Documentation about simulating a Pinball game.

- [1. Conventions and notations](#1-conventions-and-notations)
- [2. Integration](#2-integration)
- [3. Interpenetration](#3-interpenetration)
- [4. Continuous collision detection](#4-continuous-collision-detection)
  - [4.1. When collider is a point](#41-when-collider-is-a-point)
  - [4.2. When collider is a half-plane](#42-when-collider-is-a-half-plane)
  - [4.3. When collider is a disk](#43-when-collider-is-a-disk)
- [5. Continuous collision reaction](#5-continuous-collision-reaction)
- [6. After the collision](#6-after-the-collision)

## 1. Conventions and notations

This is a 2D simulation with no elevation. We assume the simulation will run in real-time at arround 60Hz, but it should work with other framerates, even if it is not perfectly constant.

Notations:
- Vectors are noted with an arrow on top: $\vec{v}\in\mathbb{R}^2$;
- Points on plane are noted with capital letters: $P\in\mathbb{R}^2$;
- Vectors from one point $A$ to another point $B$ are noted $\overrightarrow{AB}=B-A$;
- for any vector $\vec{v}\in\mathbb{R}^2$, its normalized version is noted:
  $$ \overrightarrow{\lang v\rang}=\frac{\vec{v}}{||\vec{v}||} $$

Definitions:
- the algorithm is iterative, each step is noted $i\in \mathbb{N}$ and consist of using the state $i$ in order to compute the state $i+1$
- the time at iteration $i$ is noted $t_i\in \mathbb{R}^+$
- the timestep between two consecutive iterations $i$ and $i+1$ is noted ${\Delta t}_i=t_{i+1}-t_i$
- the ball is a disk (!) of radius $r\in \mathbb{R}^{+*}$
- the 2D acceleration of the ball at time $t_i$ is noted $\vec{a}_i\in \mathbb{R}^2$
- the 2D speed of the ball at time $t_i$ is noted $\vec{s}_i\in \mathbb{R}^2$
- the 2D position of the ball at time $t_i$ is noted $X_i\in \mathbb{R}^2$
- the gravity at any position is noted $\vec{g}:\mathbb{R}^2\rightarrow\mathbb{R}^2$ (note that an elevation can be added at that point in order to simulate ramps)

When we are focusing on a specific iteration $i$, we may ommit $\square_i$ as it is implied.

## 2. Integration

Initial values (especially $\vec{x}_0$) depends on the board, but can be set like these:
$$ \vec{a}_0=0, \vec{s}_0=0, X_0=0 $$

At iteration $i$ we compute the acceleration, speed and position for the next state $i+1$ using the Euler method:

$$ \vec{a}_{i+1}=\vec{g}(\vec{x}_i) $$

$$ \vec{s}_{i+1} = \vec{s}_i + {\Delta t}_i \times \vec{a}_{i+1} $$

$$ X_{i+1} = X_i + {\Delta t}_i \times \vec{s}_{i+1} $$

## 3. Interpenetration

> TODO: It may be interesting to detect interpenetrations before the continuous collision detection.

## 4. Continuous collision detection

Collisions should be detected between $X_i$ and $X_{i+1}$.

We assume that there is no interpenetration at time $t_i$, at most the ball is touching a collider, which may be resolve as a collision with $\delta t=0$.

If a collision is detected, then the speed $\vec{s}_{i+1}$ and the position $X_{i+1}$ needs to be changed, and the contact point $C$, the time to collision $\delta t$ and the normal $\vec{n}$ of the collider at $C$ should be returned in order to allow the collision reaction algorithm to be executed.

### 4.1. When collider is a point

Given:
- the point $P\in\mathbb{R}^2$.

One create a frame local to the moving ball, centered at $\vec{x}$ with base 
$$\mathscr{B}=\left\{\vec{t},\vec{n}\right\},\vec{n}=\overrightarrow{\lang s\rang},\vec{t}=\left[-\vec{n}_y;\vec{n}_x\right]$$

Algorithm:
1. If $\overrightarrow{XP}\cdot\vec{n}\le0$ then the ball is globally moving away from that point and there couldn't be any collision.
2. Compute the projection of the point to the tangent:
   $$\overrightarrow{XC}_t=\overrightarrow{XP}\cdot\vec{t}$$
   If $||\overrightarrow{XC}_t||\notin\left]-r,r\right[$ then the ball will not touch the point.
3. Compute the projection of $\overrightarrow{XC}$ to $\vec{n}$ (we know that $||\overrightarrow{XC}||=r$):
   $$ \overrightarrow{XC}_n=\sqrt{r^2-\overrightarrow{XC}_t^2} $$
4. Then:
   $$\overrightarrow{XC}=\overrightarrow{XC}_t\times\vec{t}+\overrightarrow{XC}_n\times\vec{n}$$
5. Compute distance to point along the speed vector:
   $$ d=||\overrightarrow{CP}||=||\overrightarrow{XP}-\overrightarrow{XC}|| $$
6. Compute the time before contact:
   $$ \delta t=\frac{d}{||\vec{s}||} $$
   If $\delta t\ge\Delta t$ there will be no collision for that iteration. The particular case $\delta t=\Delta t$ will be handled in the next iteration.
7. Compute the normal of contact:
   $$ \vec{N}=-\overrightarrow{\lang{xc}\rang} $$

### 4.2. When collider is a half-plane

Given:
- a point at the surface of the half-plane $P$;
- the normal of the surface of the half-plane, pointing outside $\vec{n}$.

Algorithm:
1. If $\vec{n}\cdot\vec{s}\ge0$ then the ball is moving away from the half-plane and there will be no collision.
2. The point of collision at the surface of the ball before motion is:
   $$ C_B=X-r\vec{n} $$
3. The point of collision on the half-plane is:
   $$ C=P+\left(\frac{C_B\wedge\vec{s}-P\wedge\vec{s}}{\vec{t}\wedge\vec{s}}\right)\vec{t} $$
   > Note: At that point, we can detect that the ball is interpenetrated inside the half-plane when: $\overrightarrow{CC_B}\cdot\vec{n}<0$
4. Normal is trivially $\vec{n}$
5. Distance between the contact point on the ball and the contact point on the half-plane: 
   $$ d = ||\overrightarrow{CC_B}|| $$
6. Time to collision is:
   $$ \delta t=\frac{d}{||\vec{s}||} $$
   If $\delta t\ge\Delta t$ then the ball will not have time to be in collision with the half-plane within this iteration.

### 4.3. When collider is a disk

Given:
- the center of the disk $P$;
- the radius of the disk $r_\mathscr{D}$.

Algorithm:
1. If $\vec{s}\cdot\overrightarrow{XP}\le0$ then the ball is moving away from the disk.
2. On the ray $(X;\vec{s})$, find the closest point $D$ to $P$:
   $$ D=X+\overrightarrow{XP}\cdot\overrightarrow{\lang s\rang}\times\overrightarrow{\lang s\rang} $$
   If $||\overrightarrow{DP}||\ge r+r_\mathscr{D}$ then there will be no collision.
3. Compute the position of the ball when it is stopped along the ray $(X;\vec{s})$:
   $$ X_2=D-\sqrt{\left(r+r_\mathscr{D}\right)^2-\overrightarrow{DP}^2}\times \overrightarrow{\lang s\rang} $$
4. Compute the normal of collision:
   $$ \vec{n}=\overrightarrow{\lang PX_2\rang} $$
5. Compute the contact point:
   $$ C=X_2-r\vec{n}=P+r_\mathscr{D}\vec{n} $$

## 5. Continuous collision reaction

The collision detection algorithms should provide multiple data:
- collision or not;
- time of collision, $\delta t$;
- point of collision, $C$;
- normal of the surface at collision point, $\vec{n}$ with $||\vec{n}||=1$;
- restitution coefficient of the surface at collision point, $c$.

And we already know:
- speed $\vec{s}_{N+1}$;
- positions $X_N$ and $X_{N+1}$.

1. Virtually move the ball to the collision point $t_i+\delta t$, it gives the new position:
   $$ X_c=C+r\times\vec{n} $$
2. Compute a tangent on the surface:
   $$ \vec{t}=[-\vec{n}_y;\vec{n}_x] $$
3. "Mirror" the speed $\vec{s}_{i+1}$ on the surface, keeping the tengential component, but reflecting the normal one:
   $$ \vec{s}_{i+1}=(\vec{s}_{i+1}\cdot\vec{t})\vec{t} - c(\vec{s}_{i+1}\cdot\vec{n})\vec{n} $$
4. Finish the Euler method:
   $$ X_{i+1}=X_c+(\Delta t-\delta t)\cdot\vec{s}_{i+1} $$

## 6. After the collision

After the collision, the ball can reach another surface before $t_{i+1}$, it may be interesting to redo the process again, multiple times until convergence.

This does not treat the friction (yet!).
