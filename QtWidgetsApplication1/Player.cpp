#include "Player.h"

Player::Player(int field_x, int field_y, int height, int width,
               int creating_order)
    : height(height),
      width(width),
      x(randomint(0, field_x - width)),
      y(randomint(0, field_y - height)),
      vx(randomint(-10, 10)),
      vy(randomint(-10, 10)),
      polygon(Polygon(std::vector<Vector2>{
          Vector2(x, y), Vector2(x, y + height), Vector2(x + width, y + height),
          Vector2(x + width, y)})),
      is_alive(true),
      creating_order(creating_order),
      survival_rank(creating_order) {}

Player::Player(int field_x, int field_y, int height, int width,
               int creating_order, int combat_force_option)
    : Player(field_x, field_y, height, width, creating_order) {
  switch (combat_force_option) {
    case 0:
      int temp = randomint(0, 5);
      combat_force_level =
          temp == 0 ? (randomint(1, 7) * 2) : (randomint(1, 7) * 1);
      break;
    case 1:
      combat_force_level =
          1 + static_cast<int>(5 * 10 / PlayerNumber *
                               (PlayerNumber - (creating_order + 1))) /
                  10;
      break;
    case 2:
      double tempd = randomdouble(0, 1);
      double temp1 = randomdouble(0, 1);
      combat_force_level = randomint(1, 6);
      if (tempd < 1 / (5 + 5 * (creating_order + 1) / (PlayerNumber - 1))) {
        if (combat_force_level == 1 &&
            temp1 > ((creating_order + 1) + 1) / PlayerNumber) {
          combat_force_level = 12;
        } else {
          combat_force_level *= 2;
        }
      } else {
        if (combat_force_level == 1 &&
            temp1 > ((creating_order + 1) + 1) / PlayerNumber) {
          combat_force_level = 6;
        }
      }
      break;
    default:
      break;
  }
}

Player::Player(int field_x, int field_y, int combat_force_option,
               int creating_order)
    : Player(field_x, field_y, 15, 100, creating_order, combat_force_option) {}

void Player::move(int field_x, int field_y) {
  auto old_x = x;
  auto old_y = y;
  x += vx;
  y += vy;

  y = y < 0 ? field_y - height : (y > field_y - height ? 0 : y);
  x = x < 0 ? field_x - width : (x > field_x - width ? 0 : x);
}

void Player::update_speed() {
  vx = randomint(-10, 10);
  vy = randomint(-10, 10);
}

void Player::battle(Player& player, int kill_options) {
  if (!player.is_alive) {
    return;
  }
  if (polygon.is_cover(player.polygon)) {
    switch (kill_options) {
      case 0:
        player.hit_point -= 1;
        break;
      case 1:
        if (combat_force_level >= player.combat_force_level) {
          player.hit_point -= static_cast<int>(
              abs(combat_force_level - player.combat_force_level) / 2.5);
        }
        if (combat_force_level <= player.combat_force_level) {
          hit_point -= static_cast<int>(
              abs(combat_force_level - player.combat_force_level) / 2.5);
        }
        break;
      case 2:
        if (combat_force_level > player.combat_force_level) {
          player.hit_point -= static_cast<int>(combat_force_level);
          hit_point -= static_cast<int>(player.combat_force_level);
        } else {
          player.hit_point -= static_cast<int>(combat_force_level / 8);
          hit_point -= static_cast<int>(player.combat_force_level / 8);
        }
        break;
      case 3:
        if (combat_force_level > player.combat_force_level) {
          player.hit_point -= static_cast<int>(
              abs(combat_force_level - player.combat_force_level) / 2.5);
          hit_point += static_cast<int>(
              abs(combat_force_level - player.combat_force_level) / 2.5);
          hit_point = hit_point > 510 ? 510 : hit_point;
        } else if (combat_force_level < player.combat_force_level) {
          hit_point -= static_cast<int>(
              abs(player.combat_force_level - combat_force_level) / 2.5);
          player.hit_point += static_cast<int>(
              abs(player.combat_force_level - combat_force_level) / 2.5);
          hit_point = hit_point > 510 ? 510 : hit_point;
        } else {
          player.hit_point -= static_cast<int>(
              abs(combat_force_level - player.combat_force_level) / 2.5);
          hit_point -= static_cast<int>(
              abs(player.combat_force_level - combat_force_level) / 2.5);
        }
        break;
      default:
        break;
    }
  }

  //     OnHitPointChanged(new HitPointChangedEventArgs(HitPoint, BattleField));
  // player.OnHitPointChanged(
  //    new HitPointChangedEventArgs(player.HitPoint, BattleField));

  settle(player, 0);
  player.settle(*this, 0);
}

void Player::settle(Player& player, int survival_time) {
  if (hit_point <= 0 && is_alive) {
    survival_time = survival_time;
    survival_rank = PlayerRemainedNumber;
    killed_by = player.name;
    kill_number++;
    player.attack_score +=
        static_cast<int>(300 * sqrt((double)survival_time / 300));
    is_alive = false;
    if (PlayerRemainedNumber != 0 && is_player()) {
      PlayerRemainedNumber--;
    }
  }
}

std::vector<std::string> Player::info() {
  auto killed_by_name{!killed_by.empty() ? killed_by
                                         : (is_player ? "winner" : "none")};
  time_score = static_cast<int>(300 * sqrt(survival_time / 300));
  score = time_score + attack_score + bonus;
  return std::vector<std::string>{name,
                                  fmt::format("{}", survival_rank),
                                  fmt::format("{}", survival_time),
                                  fmt::format("{}", score),
                                  fmt::format("{}", time_score),
                                  fmt::format("{}", attack_score),
                                  fmt::format("{}", bonus),
                                  fmt::format("{}", kill_number),
                                  killed_by};
}
