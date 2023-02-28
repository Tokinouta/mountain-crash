#include "Polygon.h"

Vector2::Vector2(double x, double y) : x(x), y(y) {}

const Vector2 Vector2::operator*(const double a) {
  return Vector2(a * x, a * y);
}

const Vector2 Vector2::operator-(const Vector2& a) { return Vector2(-x, -y); }

const Vector2 Vector2::normalize() const {
  auto norm{sqrt(x * x + y * y)};
  return Vector2(x / norm, y / norm);
}

const Vector2 Vector2::perpendicular() const { return Vector2(y, -x); }

Vector2 operator*(const double a, const Vector2& b) {
  return Vector2(b.x * a, b.y * a);
}

const double operator*(const Vector2& a, const Vector2& b) {
  return a.x * b.x + a.y * b.y;
}

const Vector2 operator+(const Vector2& a, const Vector2& b) {
  return Vector2(a.x + b.x, a.y + b.y);
}

const Vector2 operator-(const Vector2& a, const Vector2& b) {
  return Vector2(a.x - b.x, a.y - b.y);
}

bool is_overlap(std::pair<double, double> a, std::pair<double, double> b) {
  if (a.first > a.second) {
    std::swap(a.first, a.second);
  }
  if (b.first > b.second) {
    std::swap(b.first, b.second);
  }
  return !(a.first > b.second || b.first > a.second);
}

Polygon::Polygon(std::vector<Vector2> vertices) : vertices(vertices) {
  n = vertices.size();
  edges = std::vector<Vector2>(n);
  for (auto i = 0; i < n - 1; i++) {
    edges[i] = vertices[i] - vertices[i + 1];
  }
  edges[n - 1] = vertices[n - 1] - vertices[0];
}

void Polygon::move(double dx, double dy) {
  for (auto& v : vertices) {
    v.x += dx;
    v.y += dy;
  }
}

void Polygon::move(Vector2 d) { move(d.x, d.y); }

std::pair<double, double> Polygon::project(Vector2 axis) const {
  axis = axis.normalize();
  auto min{vertices[0] * axis};
  auto max{min};
  for (const auto& v : vertices) {
    auto proj{v * axis};
    min = std::min(min, proj);
    max = std::max(max, proj);
  }
  return std::pair<double, double>(min, max);
}

bool Polygon::is_cover(const Polygon& b) {
  for (const auto& e : edges) {
    auto axis{e.normalize()};
    auto a_interval{project(axis)};
    auto b_interval{b.project(axis)};
    if (!is_overlap(a_interval, b_interval)) {
      return false;
    }
  }
  for (const auto& e : b.edges) {
    auto axis{e.normalize()};
    auto a_interval{project(axis)};
    auto b_interval{b.project(axis)};
    if (!is_overlap(a_interval, b_interval)) {
      return false;
    }
  }
  return true;
}
