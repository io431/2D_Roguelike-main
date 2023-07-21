using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public float moveDistance = 1f;
    public PlayerMovement player;

    private bool isMoving = false;
    private Vector3 targetPosition;

    private void Start()
    {
        player.OnMoveFinished += TryMove;
    }

    private void OnDestroy()
    {
        player.OnMoveFinished -= TryMove;
    }

    private void TryMove()
    {
        // �G���ړ����Ȃ牽�����Ȃ�
        if (isMoving) return;

        // �v���C���[�̈ʒu�ƓG�̈ʒu���^�C���}�b�v��̃Z�����W�ɕϊ�
        Vector3Int playerCell = tilemap.WorldToCell(player.transform.position);
        Vector3Int enemyCell = tilemap.WorldToCell(transform.position);

        // �������Ɉړ������݂�
        if (playerCell.x != enemyCell.x)
        {
            Vector3Int targetCell = enemyCell + new Vector3Int((int)Mathf.Sign(playerCell.x - enemyCell.x), 0, 0);

            // �ړ��悪�v���C���[�̃Z���Ȃ�U���A�����łȂ��Ȃ炻�̕����Ɉړ�����
            if (targetCell == playerCell)
            {
                AttackPlayer();
                return;
            }
            else if (player.IsWalkableTile(targetCell))
            {
                StartCoroutine(MoveToCell(targetCell));
                return;
            }
        }

        // �������Ɉړ��ł��Ȃ��ꍇ�A�c�����Ɉړ������݂�
        if (playerCell.y != enemyCell.y)
        {
            Vector3Int targetCell = enemyCell + new Vector3Int(0, (int)Mathf.Sign(playerCell.y - enemyCell.y), 0);

            // �ړ��悪�v���C���[�̃Z���Ȃ�U���A�����łȂ��Ȃ炻�̕����Ɉړ�����
            if (targetCell == playerCell)
            {
                AttackPlayer();
            }
            else if (player.IsWalkableTile(targetCell))
            {
                StartCoroutine(MoveToCell(targetCell));
            }
        }
    }

    private void EnemyTurn()
    {
        TryMove();
    }
    private void AttackPlayer()
    {

        // �v���C���[���U�����鏈��

        Debug.Log("�U�����ꂽ��");

        
    }

    private IEnumerator MoveToCell(Vector3Int targetCell)
    {
        isMoving = true;
        targetPosition = tilemap.GetCellCenterWorld(targetCell);

        // �ړ��A�j���[�V�����Ȃǂ̏�����ǉ�����ꍇ�͂����ɋL�q

        // �ړ�
        while (transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveDistance * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}
