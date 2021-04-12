using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeContainerHints : HintsController {

    /*
     * There is nowhere to place display screens without affecting the design of the puzzle's layout.
     * Thus, we hijack the rationale boards to display the hints.
     */
    [SerializeField] protected TextRollout[] rationaleBoards;

    protected override void ActivateHints() {
        base.ActivateHints();
        foreach (TextRollout board in rationaleBoards) {
            board.StartRollOut(hintText);
        }
    }

    public override void DeactivateHints() {
        base.DeactivateHints();
        foreach (TextRollout board in rationaleBoards) {
            if (board.IsRollingOut) {
                board.StopRollOut();
                board.Text += "- Oh.";
            }
        }
    }
}
