# 📜 TSCP Framework - M2 Reference Standard (v3.6)
**Exhaustive Tensor Formulation for Systemic Primitives**

**M3 Anchor:** `https://github.com/Echopraxium/tscp-framework/ontology/TSCP_M3_Ontology.jsonld`

---

## 1. Principles of Tensor Mapping
Each concept in M2 is mapped to a specific mathematical object $\mathcal{T}$ (Scalar, Vector, or Operator). The coupling between these objects defines the system's behavior.

---

## 2. The 30 Primitives & Their Tensor Formulas

### A. Topology (Manifold Definition)
1.  **Space** ($Sp$) | $\mathcal{V} \subseteq \mathbb{R}^n$ : The manifold where all states exist.
2.  **Boundary** ($B$) | $\partial \mathcal{V}$ : Hypersurface defining the agent's closure.
3.  **Environment** ($Env$) | $\mathcal{V}_{ext} = \mathcal{V} \setminus \mathcal{V}_{agent}$ : The complementary space.
4.  **System** ($Sys$) | $\mathcal{V}_{sys} = \bigoplus \mathcal{V}_i$ : Direct sum of sub-agent manifolds.

### B. Identity (Invariant Logic)
5.  **Agent** ($Ag$) | $\mathcal{A}: \mathcal{V} \to \mathcal{V}$ : The discrete operator acting on the manifold.
6.  **Identity** ($I$) | $\mathbf{I}_{inv} = \{ \vec{v} \mid \mathbf{T}\vec{v} = \vec{v} \}$ : The kernel of invariant attributes.
7.  **Facet** ($Fc$) | $\vec{v}_{\phi} = \mathbf{P}_{\phi} \vec{v}$ : Contextual projection of the identity.

### C. Physics (State & Potential)
8.  **Attribute** ($At$) | $\vec{v} \in \mathcal{V}$ : Position vector in the state-space.
9.  **Resource** ($Rs$) | $\rho = \| \vec{v}_{rs} \|$ : Scalar magnitude of a specific attribute subset.
10. **Energy** ($En$) | $E = \int \vec{F} \cdot d\vec{s}$ : Work potential available for transformation.
11. **Gradient** ($Gr$) | $\nabla \Phi = \left[ \frac{\partial \Phi}{\partial x_1}, \dots, \frac{\partial \Phi}{\partial x_n} \right]^T$ : Directional pressure field.
14. **Threshold** ($Th$) | $\theta \in \mathbb{R}$ : Heaviside step trigger $H(\|\nabla \Phi\| - \theta)$.

### D. Logic & Governance
13. **Role** ($Ro$) | $\mathbf{R} \in \mathbb{R}^{m \times n}$ : Linear map defining the functional interface.
14. **Behavior** ($Bh$) | $\mathcal{B} = \{ \mathbf{M}_k \}_{k=1}^N$ : Ordered set of transition matrices.
15. **Rule** ($Rg$) | $\sigma(\vec{v}) = [1 + e^{-(\mathbf{W}\vec{v} + b)}]^{-1}$ : Non-linear activation function.
16. **Protocol** ($Pr$) | $\mathcal{P} = \sigma \circ \mathcal{B}$ : Composition of rules and behaviors.
17. **Signal** ($Sg$) | $\vec{s}(t)$ : Time-dependent informational vector.
18. **Mode** ($Md$) | $\lambda \in \text{eig}(\mathbf{T})$ : Eigenstate defining the current regime.
19. **Objective** ($Ob$) | $\vec{v}^*$ : Target vector minimizing a loss function $\mathcal{L}(\vec{v}, \vec{v}^*)$.

### E. Semantics (Encoding)
20. **Representation** ($Rp$) | $\mathbf{Q}: \mathcal{V}_{constructive} \to \mathcal{V}_{analytical}$ : Basis change operator.
21. **Language** ($Lg$) | $\mathcal{L} = \{ \vec{s} \mid \text{Grammar}(\vec{s}) = 1 \}$ : Space of valid signal sequences.

### F. Dynamics (Coupling)
22. **Constraint** ($Ct$) | $\mathbf{C}\vec{v} = \vec{d}$ : Linear or non-linear subspace restriction.
23. **Interaction** ($Tx$) | $\mathbf{X} = \vec{a} \otimes \vec{b}$ : Outer product coupling of two agents.
24. **Transformation** ($Tr$) | $\vec{v}_{t+1} = \mathbf{T} \cdot \vec{v}_t$ : Internal endomorphism.
25. **Transduction** ($Td$) | $\mathcal{T}: \vec{s}_{in} \otimes E \to \vec{s}_{out}$ : Cross-domain mapping.
26. **Event** ($Ev$) | $\delta(t - t_0)$ : Temporal Dirac impulse triggering logic.
27. **Attractor** ($Atr$) | $\lim_{t \to \infty} \Phi^t(\vec{v}_0) = \vec{v}_{\infty}$ : Convergence point.
28. **Synergy (+)** ($Syn$) | $\langle \vec{a}, \vec{b} \rangle > 0$ : Constructive inner product alignment.
29. **Synergy (-)** ($Ant$) | $\langle \vec{a}, \vec{b} \rangle < 0$ : Destructive inner product (Antagonism).

### G. Time
30. **Temporality** ($\Phi$) | $t \in \mathbb{R}^+$ : The monotonic scalar flow.