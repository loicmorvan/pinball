# pinball-physics
 Documentation about simulating a Pinball game

## Integration

Given a constant gravity $\vec{g}$.

$$ E1: \overrightarrow{a_{N+1}}=\vec{g} $$
$$ E2: \overrightarrow{v_{N + 1}} = \overrightarrow{v_N}+ \Delta t \times \overrightarrow{a_{N + 1}} $$
$$ E3: \overrightarrow{x_{N+1}} = \overrightarrow{x_N} +  \Delta t \times \overrightarrow{v_{N+1}} $$
