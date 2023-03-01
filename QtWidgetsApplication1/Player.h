#pragma once
#include <fmt/core.h>

#include <QTimer>
#include <optional>
#include <random>
#include <string>
#include <typeinfo>
#include <vector>

#include "Polygon.h"

static int randomint(int min, int max) {
  static std::random_device
      rd;  // Will be used to obtain a seed for the random number engine
  static std::mt19937 gen(
      rd());  // Standard mersenne_twister_engine seeded with rd()
  static std::uniform_int_distribution<int> distrib(min, max);
  return distrib(gen);
}

static double randomdouble(int min, int max) {
  static std::random_device
      rd;  // Will be used to obtain a seed for the random number engine
  static std::mt19937 gen(
      rd());  // Standard mersenne_twister_engine seeded with rd()
  static std::uniform_real_distribution<> distrib(min, max);
  return distrib(gen);
}

struct Player {
  static int PlayerNumber;
  static int PlayerRemainedNumber;

  std::string name{""};
  Player* killed_by{nullptr};

  // 运动学参数
  // 这里的x和y以窗口左上角为原点，向右为x，向下为y
  int width{100};
  int height{15};
  int x{0};  // 左上角的x坐标
  int y{0};  // 左上角的y坐标
  int vx{0};
  int vy{0};
  double speed() const { return sqrt(vx * vx + vy * vy); }
  Polygon polygon;

  // 战斗相关参数
  int hit_point{510};
  double combat_force_level{0};
  bool is_alive{false};

  // 结算参数
  int creating_order{0};
  int score{0};
  int kill_number{0};
  int survival_rank{0};
  int time_score{0};
  int attack_score{0};
  int bonus{0};
  int survival_time{0};

  // 其他
  bool is_player() const { return typeid(this) == typeid(Player); }

  Player(int field_x, int field_y, int height, int width, int creating_order);
  Player(int field_x, int field_y, int height, int width, int creating_order,
         int combat_force_option);
  Player(int field_x, int field_y, int combat_force_option, int creating_order);

  // 在速度方向上移动
  // 如果碰到边界就移动到另一边
  void move(int field_x, int field_y);
  // 速度随机更新
  void update_speed();
  // 和另一个玩家战斗，看看如何掉血
  void battle(Player& player, int kill_options);
  // 结算，和另一个玩家
  virtual void settle(Player& player, int survival_time);
  // 玩家信息，返回一个字符串数组
  std::vector<std::string> info();
};

struct Hat : Player {
  int count_down;
  std::unique_ptr<QTimer> count_down_timer;

  Hat(int field_x, int field_y, int combat_force_option);
  void timer_count_down_tick();
  void settle(Player& player, int survival_time) override;
};

struct Egg : Player {};

struct Elf : Player {
  int period_of_recharge;
  Player* player_protected_by_elf;
  std::unique_ptr<QTimer> timer_of_protection;
  std::unique_ptr<QTimer> timer_of_recharge;
  Elf(int field_x, int field_y, int combat_force_option);
};
struct Proprieter : Player {};

struct Ozone : Player {};